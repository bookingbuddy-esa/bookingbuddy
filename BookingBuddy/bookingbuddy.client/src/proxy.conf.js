const PROXY_CONFIG = [
  {
    context: [
      "/api",
      "/login",
    ],
    target: "https://bookingbuddy-api-server.azurewebsites.net/",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
