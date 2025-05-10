// wwwroot/js/messageService.js
class MessageService {
    constructor() {
        this.listeners = {};
    }

    // Register a listener for a specific message type
    addListener(messageType, callback) {
        if (!this.listeners[messageType]) {
            this.listeners[messageType] = [];
        }
        this.listeners[messageType].push(callback);
    }

    // Remove a listener
    removeListener(messageType, callback) {
        if (!this.listeners[messageType]) return;
        this.listeners[messageType] = this.listeners[messageType].filter(cb => cb !== callback);
    }

    // Send a message to all listeners of the specified type
    sendMessage(messageType, parameters) {
        const parsedObject = JSON.parse(parameters);
        if (this.listeners[messageType]) {
            this.listeners[messageType].forEach(callback => callback(parsedObject));
        }
    }
}
