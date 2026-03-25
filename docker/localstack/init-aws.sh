#!/bin/bash
set -e

echo "==> Initializing LocalStack AWS resources..."

AWS_ENDPOINT="http://localhost:4566"
AWS_REGION="us-east-1"
AWS_ARGS="--endpoint-url=$AWS_ENDPOINT --region=$AWS_REGION"

# ─── SQS Queues ───────────────────────────────────────────────────────────────

echo "Creating SQS queues..."

# Dead Letter Queue (must be created first)
aws sqs create-queue $AWS_ARGS \
  --queue-name finance-dlq \
  --attributes '{"MessageRetentionPeriod":"1209600"}'

DLQ_ARN=$(aws sqs get-queue-attributes $AWS_ARGS \
  --queue-url http://localhost:4566/000000000000/finance-dlq \
  --attribute-names QueueArn \
  --query 'Attributes.QueueArn' --output text)

# Main queues with DLQ redrive policy
aws sqs create-queue $AWS_ARGS \
  --queue-name transaction-events \
  --attributes "{\"RedrivePolicy\":\"{\\\"deadLetterTargetArn\\\":\\\"$DLQ_ARN\\\",\\\"maxReceiveCount\\\":\\\"3\\\"}\"}"

aws sqs create-queue $AWS_ARGS \
  --queue-name notification-events \
  --attributes "{\"RedrivePolicy\":\"{\\\"deadLetterTargetArn\\\":\\\"$DLQ_ARN\\\",\\\"maxReceiveCount\\\":\\\"3\\\"}\"}"

aws sqs create-queue $AWS_ARGS \
  --queue-name account-events

echo "SQS queues created."

# ─── SNS Topics ───────────────────────────────────────────────────────────────

echo "Creating SNS topics..."

aws sns create-topic $AWS_ARGS --name account-events-topic
aws sns create-topic $AWS_ARGS --name transaction-events-topic
aws sns create-topic $AWS_ARGS --name notification-topic

echo "SNS topics created."

# ─── S3 Buckets ───────────────────────────────────────────────────────────────

echo "Creating S3 buckets..."

aws s3 mb s3://finance-kyc-documents $AWS_ARGS
aws s3 mb s3://finance-statements $AWS_ARGS

# Enable versioning on KYC documents bucket
aws s3api put-bucket-versioning $AWS_ARGS \
  --bucket finance-kyc-documents \
  --versioning-configuration Status=Enabled

echo "S3 buckets created."

# ─── Secrets Manager ──────────────────────────────────────────────────────────

echo "Creating secrets..."

aws secretsmanager create-secret $AWS_ARGS \
  --name finance/db-credentials \
  --secret-string '{"host":"postgres","port":5432,"database":"financedb","username":"finance","password":"finance"}'

aws secretsmanager create-secret $AWS_ARGS \
  --name finance/keycloak-credentials \
  --secret-string '{"admin":"admin","password":"admin"}'

echo "Secrets created."

# ─── SES Email Identities ─────────────────────────────────────────────────────

echo "Verifying SES email identities..."

aws ses verify-email-identity $AWS_ARGS --email-address noreply@financeapp.local
aws ses verify-email-identity $AWS_ARGS --email-address support@financeapp.local

echo "SES identities created."

echo "==> LocalStack initialization complete!"
