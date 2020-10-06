import * as SignalR from "@microsoft/signalr"

export function StartSocket(token) {
  const connection = new SignalR.HubConnectionBuilder()
    .withUrl(`${window.config.Url}/game?access_token=${token}`)
    .configureLogging(SignalR.LogLevel.Information)
    .build();

  return connection.start().then(() => connection)
}

export function Authenticate(username, password) {
  return fetch(`${window.config.Url}/user/authentication`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ username: username, password: password })
  })
    .then(response => {
      if (response.status == 400) {
        throw "Unauthorized"
      }
      return response.json()
    })
}