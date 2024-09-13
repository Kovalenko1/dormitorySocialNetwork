import * as signalR from '@microsoft/signalr';

let connection = null;

export const getConnection = () => connection;

export const connectToChatHub = async (dispatch, receiveMessage) => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl('http://localhost:5000/chat')
        .withAutomaticReconnect()
        .build();

    connection.on('ReceiveMessage', (userName, message) => {
        dispatch(receiveMessage({ userName, message }));
    });

    try {
        await connection.start();
        return { connectionStatus: 'connected' };
    } catch (error) {
        console.error('Connection failed:', error);
        throw error;
    }
};

export const joinPrivateChat = async (userId1, userId2) => {
    if (!connection) throw new Error('Not connected');
    await connection.invoke('JoinPrivateChat', userId1, userId2);
};

export const sendMessageToPrivateChat = async (user1Id, user2Id, message) => {
    if (!connection) throw new Error('Not connected');
    await connection.invoke('SendMessageToPrivateChat', user1Id, user2Id, message);
};
