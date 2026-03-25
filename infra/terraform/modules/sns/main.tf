resource "aws_sns_topic" "account_events" {
  name = "account-events-topic"

  tags = {
    Environment = var.environment
    Application = "finance-app"
  }
}

resource "aws_sns_topic" "transaction_events" {
  name = "transaction-events-topic"

  tags = {
    Environment = var.environment
    Application = "finance-app"
  }
}

resource "aws_sns_topic" "notifications" {
  name = "notification-topic"

  tags = {
    Environment = var.environment
    Application = "finance-app"
  }
}
