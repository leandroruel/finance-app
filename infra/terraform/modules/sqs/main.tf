resource "aws_sqs_queue" "dlq" {
  name                      = "finance-dlq"
  message_retention_seconds = 1209600 # 14 days

  tags = {
    Environment = var.environment
    Application = "finance-app"
  }
}

resource "aws_sqs_queue" "transaction_events" {
  name                       = "transaction-events"
  visibility_timeout_seconds = 30
  message_retention_seconds  = 86400

  redrive_policy = jsonencode({
    deadLetterTargetArn = aws_sqs_queue.dlq.arn
    maxReceiveCount     = 3
  })

  tags = {
    Environment = var.environment
    Application = "finance-app"
  }
}

resource "aws_sqs_queue" "notification_events" {
  name                       = "notification-events"
  visibility_timeout_seconds = 30
  message_retention_seconds  = 86400

  redrive_policy = jsonencode({
    deadLetterTargetArn = aws_sqs_queue.dlq.arn
    maxReceiveCount     = 3
  })

  tags = {
    Environment = var.environment
    Application = "finance-app"
  }
}

resource "aws_sqs_queue" "account_events" {
  name                       = "account-events"
  visibility_timeout_seconds = 30

  tags = {
    Environment = var.environment
    Application = "finance-app"
  }
}
