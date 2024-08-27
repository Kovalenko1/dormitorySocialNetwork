import {useState} from "react";

import style from './MessagePanel.module.scss'
import AddImg from '../../../../../assets/img/Add.svg';
import SmileImg from '../../../../../assets/img/Smile.svg';
import SendImg from '../../../../../assets/img/Send.svg';


export const MessagePanel = () => {
    const startMessage = 'Написать сообщение...'
    const [placeholder, setPlaceholder] = useState(startMessage);

    return (
        <div className={style.message_panel}>
            <div className={style.input_container}>
                <input
                    className={style.message_input}
                    placeholder={placeholder}
                    onFocus={() => setPlaceholder('')}
                    onBlur={() => setPlaceholder(startMessage)}
                    type="text"/>
                <div className={style.nav_message_bar}>
                    <img id="addImg" src={AddImg} alt=""/>
                    <img id="smileImg" src={SmileImg} alt=""/>
                    <img id="sendImg" src={SendImg} alt=""/>
                </div>
            </div>
        </div>
    );
}