import style from './MessageBar.module.scss';
import { useSelector } from 'react-redux';
import { useEffect } from "react";

export const MessageBar = () => {
    const messages = useSelector(state => state.chat.messages);
    console.log("message", messages);
    const selectedChatId = useSelector(state => state.selectChat.selectedChatId);
    const currentChatRoom = [JSON.parse(localStorage.getItem('user')).id, selectedChatId]
        .sort((a, b) => a - b)[0]
        + "-" +
        [JSON.parse(localStorage.getItem('user')).id, selectedChatId].sort((a, b) => a - b)[1];

    useEffect(() => {
        if (currentChatRoom && messages[currentChatRoom]) {
            localStorage.setItem(currentChatRoom, JSON.stringify(messages[currentChatRoom]));
        }
    }, [currentChatRoom, messages]);

    return (
        <div className={style.message_bar}>
            {messages[currentChatRoom] && messages[currentChatRoom].length > 0 ? (
                messages[currentChatRoom].map((msg, index) => (
                    <div key={index} className={style.message}>
                        {msg.file && Array.isArray(msg.file) && (
                            <div>
                                {msg.file.map((file, fileIndex) => (
                                    <div key={fileIndex}>
                                        {/\.(jpeg|jpg|gif|png|svg|webp)$/i.test(file) ? (
                                            <img src={file} alt={`file-${fileIndex + 1}`} />
                                        ) : (
                                            <a
                                                href={file}
                                                className={style.text}
                                                target="_blank"
                                                rel="noopener noreferrer"
                                            >
                                                Скачать файл {fileIndex + 1}
                                            </a>
                                        )}
                                    </div>
                                ))}
                            </div>
                        )}
                        {msg.text && <div className={style.text}>{msg.text}</div>}
                    </div>
                ))
            ) : (
                <div className={style.no_messages}>
                    Нет сообщений в этом чате.
                </div>
            )}
        </div>
    );
};
