variable "environment" {
  description = "Deployment environment"
  type        = string
  default     = "local"
}

variable "sender_email" {
  description = "Primary sender email address"
  type        = string
}
