import axios from 'axios';

const API_KEY = 'TB46Y2SQFVGPS4U7';

export default {
  async getStockData(symbol) {
    const apiKey = "TB46Y2SQFVGPS4U7";
    const url = `https://www.alphavantage.co/query?function=TIME\_SERIES\_INTRADAY&symbol=${this.symbol}&interval=60min&apikey=${apiKey}`;

    let timeSeries = null;

      while (!timeSeries) {
        try {
          const response = await axios.get(url);
          timeSeries = response.data["Time Series (60min)"];

          if (!timeSeries) {
            await new Promise(resolve => setTimeout(resolve, 10000)); // ждем 10 секунд перед следующим запросом
          }
        } catch (error) {
          console.error("Error fetching stock data:", error);
          await new Promise(resolve => setTimeout(resolve, 10000)); // ждем 10 секунд перед следующим запросом
        }
      }

    const data = await response.json();
    timeSeries = data['Time Series (60min)'];

    // console.log(data, timeSeries);
    // const dates = Object.keys(timeSeries).slice(0, 24).reverse();
    const prices = dates.map((date) => parseFloat(timeSeries[date]['4. close']));
    // const dates = [];
    // const prices = [];
    // for (const time in timeSeries) {
    //   dates.push(time.split(" ")[1].split(":")[0]);
    //   prices.push(parseFloat(timeSeries[time]["4. close"]));
    // }

    return {
      price: prices[prices.length - 1],
      // dates,
      // prices,
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

// import axios from 'axios';
// import fs from 'fs';

// const API_KEY = 'TB46Y2SQFVGPS4U7';

// export default {
//   async getStockData(symbol) {
//     const apiKey = "TB46Y2SQFVGPS4U7";
//     const url = `https://www.alphavantage.co/query?function=TIME\_SERIES\_INTRADAY&symbol=${symbol}&interval=60min&apikey=${apiKey}`;

//     let timeSeries = null;

//     while (!timeSeries) {
//       try {
//         const response = await axios.get(url);
//         timeSeries = response.data["Time Series (60min)"];

//         if (!timeSeries) {
//           await new Promise(resolve => setTimeout(resolve, 10000)); // ждем 10 секунд перед следующим запросом
//         }
//       } catch (error) {
//         console.error("Error fetching stock data:", error);
//         await new Promise(resolve => setTimeout(resolve, 10000)); // ждем 10 секунд перед следующим запросом
//       }
//     }

//     console.log('TimeSeries ', timeSeries);
//     const data = {
//       [symbol]: timeSeries,
//     };

//     fs.writeFileSync('stockData.json', JSON.stringify(data, null, 2));

//     const prices = Object.keys(timeSeries).map((date) => parseFloat(timeSeries[date]['4. close']));

//     return {
//       price: prices[prices.length - 1],
//     };
//   },
//   async fetchStockPrice(symbol) {
//     try {
//       const response = await axios.get(BASE_URL, {
//         params: {
//           function: 'GLOBAL_QUOTE',
//           symbol: symbol,
//           apikey: API_KEY,
//         },
//       });

//       if (response.data && response.data['Global Quote']) {
//         return response.data['Global Quote']['05. price'];
//       } else {
//         throw new Error('Invalid response from API');
//       }
//     } catch (error) {
//       console.error('Error fetching stock price:', error);
//       return 0;
//     }
//   },
// };
