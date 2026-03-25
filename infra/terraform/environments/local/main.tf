terraform {
  required_version = ">= 1.6"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region                      = "us-east-1"
  access_key                  = "test"
  secret_key                  = "test"
  skip_credentials_validation = true
  skip_metadata_api_check     = true
  skip_requesting_account_id  = true

  endpoints {
    rds            = "http://localhost:4566"
    sqs            = "http://localhost:4566"
    sns            = "http://localhost:4566"
    s3             = "http://localhost:4566"
    ses            = "http://localhost:4566"
    secretsmanager = "http://localhost:4566"
    cloudwatch     = "http://localhost:4566"
    logs           = "http://localhost:4566"
  }
}

module "sqs" {
  source = "../../modules/sqs"
}

module "sns" {
  source = "../../modules/sns"
}

module "s3" {
  source = "../../modules/s3"
}

module "ses" {
  source = "../../modules/ses"

  sender_email = "noreply@financeapp.local"
}

module "secrets" {
  source = "../../modules/secrets"

  db_host     = "localhost"
  db_port     = 5432
  db_name     = "financedb"
  db_username = "finance"
  db_password = "finance"
}
