# WpfApp1
# Cryptocurrency Application

## Overview

This multi-page WPF application provides a comprehensive interface for interacting with cryptocurrency data. Developed in C# and utilizing the .NET framework, this application fetches data from the CoinCap API and offers features like viewing top cryptocurrencies, detailed currency information, conversion between currencies, and historical price charts.

## Features

- **Top Currencies**: Display the top N cryptocurrencies by popularity, with dynamic configuration for the number of displayed items.
- **Currency Details**: View detailed information about a selected currency, including price, volume, price change, and market availability.
- **Currency Conversion**: Convert amounts between different currencies using real-time exchange rates.
- **Historical Price Charts**: Visualize historical price data with charts.
- **Search Functionality**: Search for cryptocurrencies by name or code.
- **Theme Support**: Toggle between light and dark themes.
- **Localization**: Support for multiple languages (if implemented).


### Prerequisites

Before you begin, ensure you have met the following requirements:

- .NET SDK (version 8.0 or later)
- Visual Studio 2022 or any other compatible C# IDE
- An internet connection to fetch API data

  API Usage
The application interacts with the CoinCap API to retrieve cryptocurrency data. Here are some key endpoints:

Base URL: https://api.coincap.io/v2/

Endpoints:

GET /assets: Fetch a list of top cryptocurrencies.
GET /assets/{id}: Retrieve detailed information about a specific currency.
GET /rates/{fromCurrency}: Get exchange rates for a given currency.
GET /assets/{currencyId}/history?interval=d1: Fetch historical price data for a currency. 

Usage

Currency Conversion
Enter the amount you want to convert.
Select the base currency and target currency from the dropdowns.
Click "Convert" to see the result.
Viewing Currency Details
Navigate to the "Currency Details" page.
Select a currency from the list or search for a specific currency.
View detailed information including price, volume, and market data.
Historical Charts
Navigate to the "Historical Charts" page.
Select a currency and time range.
View the historical price data represented in a chart.

Theme Switching
Use the theme toggle button to switch between light and dark modes.
