import * as signalR from '@microsoft/signalr';

let connection = null;

export const getConnection = () => connection;

export const connectToChatHub = async (dispatch, receiveMessage) => {

    const userId = JSON.parse(localStorage.getItem('user')).id;

    connection = new signalR.HubConnectionBuilder()
        .withUrl(`http://localhost:5000/chat?userId=${userId}`)
        .withAutomaticReconnect()
        .build();

    connection.on('ReceiveMessage', (chatId, message) => {
        console.log("chats", chatId, message);
        dispatch(receiveMessage({ chatId, message }));
    });

    try {
        await connection.start();
        return { connectionStatus: 'connected' };
    } catch (error) {
        console.error('Connection failed:', error);
        throw error;
    }
};

