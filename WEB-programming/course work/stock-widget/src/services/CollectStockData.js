import axios from 'axios';
import fs from 'fs';

const API_KEY = 'TB46Y2SQFVGPS4U7';
const symbols = ['AAPL', 'GOOGL', 'MSFT']; // Замените на список символов, которые вам нужны

async function fetchStockData(symbol) {
  const url = `https://www.alphavantage.co/query?function=TIME\_SERIES\_DAILY&symbol=${symbol}&apikey=${API_KEY}`;

  try {
    const response = await axios.get(url);
    const timeSeries = response.data['Time Series (Daily)'];

    if (timeSeries) {
      return { [symbol]: timeSeries };
    } else {
      throw new Error('Invalid response from API');
    }
  } catch (error) {
    console.error('Error fetching stock data for ${symbol}:', error);
    return null;
  }
}

async function saveStockDataToFile() {
  const stockData = {};

  for (const symbol of symbols) {
    const data = await fetchStockData(symbol);

    if (data) {
      Object.assign(stockData, data);
    }

    await new Promise(resolve => setTimeout(resolve, 10000)); // ждем 10 секунд перед следующим запросом
  }

  fs.writeFileSync('stockData.json', JSON.stringify(stockData, null, 2));
  console.log('Stock data saved to stockData.json');
}

saveStockDataToFile();
