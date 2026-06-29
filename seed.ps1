sqlcmd -S "localhost\SQLEXPRESS" -d "TrainingTrackerDB" -i ".\seed.sql" -E -f 65001
Write-Host "Seed klar!"
