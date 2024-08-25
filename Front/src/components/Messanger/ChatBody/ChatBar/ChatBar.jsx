import style from './ChatBar.module.scss';
import {MessageBar} from "./MessageBar/MessageBar";
import {MessagePanel} from "./MessagePanel/MessagePanel";

export const ChatBar = () => {
    return (
        <div className={style.chat_bar}>
            <MessageBar/>
            <MessagePanel/>
        </div>
    );
}