Start-Process powershell -ArgumentList '-NoExit', '-Command', 'cd ".\backend\Training-tracker-backend\API"; dotnet run --launch-profile https'
Start-Process powershell -ArgumentList '-NoExit', '-Command', 'cd ".\frontend"; npm start'
