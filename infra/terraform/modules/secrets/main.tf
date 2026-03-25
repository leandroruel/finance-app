resource "aws_secretsmanager_secret" "db_credentials" {
  name                    = "finance/db-credentials"
  recovery_window_in_days = 0 # immediate deletion for local dev

  tags = {
    Environment = var.environment
    Application = "finance-app"
  }
}

resource "aws_secretsmanager_secret_version" "db_credentials" {
  secret_id = aws_secretsmanager_secret.db_credentials.id
  secret_string = jsonencode({
    host     = var.db_host
    port     = var.db_port
    database = var.db_name
    username = var.db_username
    password = var.db_password
  })
}

resource "aws_secretsmanager_secret" "keycloak_credentials" {
  name                    = "finance/keycloak-credentials"
  recovery_window_in_days = 0

  tags = {
    Environment = var.environment
    Application = "finance-app"
  }
}
