

var prices = [];
var ranking = [
    "BTCUSDC",
    "ETHUSDC",
    "DOGEUSDC",
    "LTCUSDC",
    "XRPUSDC",
    "ADAUSDC"
]
document.addEventListener("DOMContentLoaded", function (event) {

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/prices")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    async function start() {
        try {
            await connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    };

    connection.onclose(async () => {
        await start();
    });

    // Start the connection.
    start();

    connection.on("UpdatePrice", async price => {
        await updatePrice(price);
        await showPrices();
    });

});

const updatePrice = async (price) => {
    var oldPrice = prices.filter(function (el) {
        return el.key == price.key
    });

    price.rank = ranking.indexOf(price.symbol) + 1;

    if (oldPrice.length > 0) {

        //Find index of specific object using findIndex method.    
        var objIndex = prices.findIndex((obj => obj.key == price.key));
        prices[objIndex].lastPrice = price.lastPrice;
    }
    else {
        prices.push(price);
    }
    prices.sort(function (a, b) {
        return parseInt(a.rank) - parseInt(b.rank);
    });
}

const showPrices = async () => {

    document.getElementById("priceTable").querySelector('tbody').innerHTML="";
    prices.forEach(price => {
        const tr = document.createElement("tr");

        const tdPersianName = document.createElement("td");
        tdPersianName.className = "coin_icon";
        
        const icon = document.createElement("i");
        icon.className = "cc " + price.symbol.replace("USDC", "").toUpperCase();
        tdPersianName.appendChild(icon);

        const spanPersianName = document.createElement("span");
        spanPersianName.innerHTML = " " + price.persianName;
        tdPersianName.appendChild(spanPersianName);

        tr.appendChild(tdPersianName);

        const tdsymbol = document.createElement("td");
        tdsymbol.innerHTML = price.symbol.replace("USDC", "").toUpperCase();
        tr.appendChild(tdsymbol);

        const tdpriceirr = document.createElement("td");
        tdpriceirr.className = "pricestyle";
        tdpriceirr.textContent = toPriceFormat(parseInt(price.lastPriceIRR), 0);
        tr.appendChild(tdpriceirr);

        const tdprice = document.createElement("td");
        tdprice.textContent = toPriceFormat(price.lastPrice, 2);
        tr.appendChild(tdprice);

        var tbody = document.getElementById("priceTable").querySelector('tbody');
        tbody.appendChild(tr);
    });
}

const toPriceFormat = (number, fractionDigits) => {
    return number.toLocaleString(undefined, {
        minimumfractiondigits: fractionDigits,
        maximumfractiondigits: fractionDigits
    });
};

const getPriceChangeStyle = (pricechangepercent) => {
    return pricechangepercent > 0 ? 'text-success' :
        pricechangepercent < 0 ? 'text-danger' : '';
}
