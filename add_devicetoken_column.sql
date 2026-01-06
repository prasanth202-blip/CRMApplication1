-- Add DeviceToken column to Users table
ALTER TABLE Users 
ADD DeviceToken NVARCHAR(MAX) NULL;