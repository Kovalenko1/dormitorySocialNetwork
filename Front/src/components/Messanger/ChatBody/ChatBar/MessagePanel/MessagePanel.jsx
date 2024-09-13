import { useEffect, useState } from "react";
import { useDispatch, useSelector } from 'react-redux';
import style from './MessagePanel.module.scss';
import AddImg from '../../../../../assets/img/Add.svg';
import SmileImg from '../../../../../assets/img/Smile.svg';
import SendImg from '../../../../../assets/img/Send.svg';
import { sendMessageToPrivateChat, joinPrivateChat } from "../../../../../store/slices/chatSlice";
import { useSelectedChat } from "../../../../../hooks/useSelectedChat";

export const MessagePanel = () => {

    const startMessage = 'Написать сообщение...';
    const [placeholder, setPlaceholder] = useState(startMessage);
    const [message, setMessage] = useState('');
    const { selectedChatId } = useSelectedChat();
    const { messages, currentChatRoom, connectionStatus } = useSelector(state => state.chat);
    const dispatch = useDispatch();

    const handleSendMessage = async () => {
        if (message.trim()) {
            try {
                await dispatch(joinPrivateChat({ userId1: parseInt(localStorage.getItem('id')), userId2: parseInt(selectedChatId) })).unwrap();
                await dispatch(sendMessageToPrivateChat({
                    user1Id: parseInt(localStorage.getItem('id')),
                    user2Id: selectedChatId,
                    message,
                })).unwrap();
                setMessage('');
            } catch (error) {
                console.error('Error sending message:', error);
            }
        }
    };

    return (
        <div className={style.message_panel}>
            <div className={style.input_container}>
                <input
                    className={style.message_input}
                    placeholder={placeholder}
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    onFocus={() => setPlaceholder('')}
                    onBlur={() => setPlaceholder(startMessage)}
                    type="text"
                    onKeyPress={(e) => e.key === 'Enter' && handleSendMessage()}
                />
                <div className={style.nav_message_bar}>
                    <img id="addImg" src={AddImg} alt="" />
                    <img id="smileImg" src={SmileImg} alt="" />
                    <img id="sendImg" src={SendImg} alt="" onClick={handleSendMessage} />
                </div>
            </div>
        </div>
    );
};
