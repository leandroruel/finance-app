resource "aws_s3_bucket" "kyc_documents" {
  bucket = "finance-kyc-documents"

  tags = {
    Environment = var.environment
    Application = "finance-app"
    Purpose     = "KYC document storage"
  }
}

resource "aws_s3_bucket_versioning" "kyc_documents" {
  bucket = aws_s3_bucket.kyc_documents.id
  versioning_configuration {
    status = "Enabled"
  }
}

resource "aws_s3_bucket" "statements" {
  bucket = "finance-statements"

  tags = {
    Environment = var.environment
    Application = "finance-app"
    Purpose     = "Account statements"
  }
}

resource "aws_s3_bucket_lifecycle_configuration" "statements" {
  bucket = aws_s3_bucket.statements.id

  rule {
    id     = "archive-old-statements"
    status = "Enabled"

    transition {
      days          = 90
      storage_class = "STANDARD_IA"
    }

    expiration {
      days = 365
    }
  }
}
