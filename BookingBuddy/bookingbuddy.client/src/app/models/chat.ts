export interface Chat {
  chatId: string,
  name: string,
  lastMessages: Message[]
}

export interface Message {
  messageId: string,
  applicationUser: { id: string, name: string },
  content: string,
  "sentAt": Date
}
