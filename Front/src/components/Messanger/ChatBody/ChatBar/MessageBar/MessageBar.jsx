import style from './MessageBar.module.scss';
import { useSelector } from 'react-redux';

export const MessageBar = () => {
    const messages = useSelector(state => state.chat.messages);
    const currentChatRoom = useSelector(state => state.chat.currentChatRoom);

    return (
        <div className={style.message_bar}>
            {messages[currentChatRoom] && messages[currentChatRoom].map((msg, index) => (
                <div key={index} className={style.message}>
                    <strong>{msg.userName}:</strong> {msg.message}
                </div>
            ))}
        </div>
    );
};
