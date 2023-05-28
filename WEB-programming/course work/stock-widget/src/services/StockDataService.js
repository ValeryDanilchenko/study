import axios from 'axios';

const API_KEY = 'TB46Y2SQFVGPS4U7';

export default {
  async getStockData(symbol) {
    const response = await fetch(
      `https://www.alphavantage.co/query?function=TIME\_SERIES\_INTRADAY&symbol=${symbol}&interval=60min&apikey=${API_KEY}`
    );
    const data = await response.json();
    const timeSeries = data['Time Series (60min)'];

    const dates = Object.keys(timeSeries).slice(0, 24).reverse();
    const prices = dates.map((date) => parseFloat(timeSeries[date]['4. close']));

    return {
      price: prices[prices.length - 1],
      dates,
      prices,
    };
  },
  async fetchStockPrice(symbol) {
    try {
      const response = await axios.get(BASE_URL, {
        params: {
          function: 'GLOBAL_QUOTE',
          symbol: symbol,
          apikey: API_KEY,
        },
      });

      if (response.data && response.data['Global Quote']) {
        return response.data['Global Quote']['05. price'];
      } else {
        throw new Error('Invalid response from API');
      }
    } catch (error) {
      console.error('Error fetching stock price:', error);
      return 0;
    }
  },
};