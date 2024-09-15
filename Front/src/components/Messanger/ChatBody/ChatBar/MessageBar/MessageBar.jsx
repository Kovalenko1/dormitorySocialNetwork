import style from './MessageBar.module.scss';
import { useSelector } from 'react-redux';

export const MessageBar = () => {
    const messages = useSelector(state => state.chat.messages);
    const currentChatRoom = useSelector(state => state.chat.currentChatRoom);

    return (
        <div className={style.message_bar}>
            {messages[currentChatRoom] && messages[currentChatRoom].map((msg, index) => (
                <div key={index} className={style.message}>
                    {msg.message.file && Array.isArray(msg.message.file) && (
                        <div>
                            {msg.message.file.map((file, fileIndex) => (
                                <div key={fileIndex}>
                                    {file.match(/\.(jpeg|jpg|gif|png)$/i) ? (
                                        <img src={file} alt="" />
                                    ) : (
                                        <a href={file} className={style.text} target="_blank" rel="noopener noreferrer">Скачать файл {fileIndex + 1}</a>
                                    )}
                                </div>
                            ))}
                        </div>
                    )}
                    {msg.message.text && <div className={style.text}>{msg.message.text}</div>}
                </div>
            ))}
        </div>
    );
};
