# LiveCryptoePrice
Live crypto price from the Coinex exchange using signalR. 
After runnig the code, you can get prices from "/hubs/prices" url. For example if your local host addres is "https://localhost:5000" then price address will be "https://localhost:5000/hubs/prices".




To explain more about how it works:

There is a **hub** that the client connects to it and a **provider** which provides data.

In the hub(_PriceHub_) you can find two methods: **OnConnectedAsync** and **OnDisconnectedAsync**. It is clear from their names when they are called. You don't need to call them. they will call automatically when the client connects or disconnects.

The provider(_PriceDataProvider_) is Injected into the hub and in both hub's methods, we call the appropriate method of provider and also an action to be called on the client side and receiving price model(_"UpdatePrice" in this case_).
