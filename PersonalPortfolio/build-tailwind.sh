#!/bin/bash
# PersonalPortfolio/build-tailwind.sh

# Create the css directory if it doesn't exist
mkdir -p wwwroot/css

# Process Tailwind CSS
npx tailwindcss -i ./Styles/tailwind.css -o ./wwwroot/css/tailwind.css

echo "Tailwind CSS processed successfully!"