import styles from './Sidebar.module.scss';
import SearchImg from '../../../assets/img/Search.svg';
import ProfileImg from '../../../assets/img/Person.svg';
import {useCallback, useState} from "react";
import { useDispatch, useSelector } from "react-redux";
import { searchUsers } from '../../../store/slices/searchSlice';
import { setSelectedChat } from "../../../store/slices/selectChatSlice";
import { debounce } from "lodash";
import {useSelectedChat} from "../../../hooks/useSelectedChat";

export const Sidebar = () => {
    const [isFocused, setIsFocused] = useState(false);
    const [selectedFolder, setSelectedFolder] = useState('chats');

    const dispatch = useDispatch();
    const searchedUsers = useSelector(state => state.search.list);
    const { selectedChatId, bodyName } = useSelectedChat();

    const debouncedSearch = debounce((query) => {
        dispatch(searchUsers(query));
    }, 800);

    const handleChatSelection = (chatId, name) => {
        dispatch(setSelectedChat({chatId, name}));
    };

    return (
        <aside className={styles.chat_sidebar}>
            <div className={styles.search_bar_container}>
                <div className={styles.search_bar}>
                    <div className={styles.search_img_container}>
                        <img className={styles.search_img} src={SearchImg} alt="" />
                    </div>
                    <input
                        type="text"
                        onFocus={() => setIsFocused(true)}
                        onBlur={() => setIsFocused(false)}
                        onChange={(e) => debouncedSearch(e.target.value)}
                        className={isFocused ? styles.search_focused : styles.search}
                    />
                </div>
            </div>
            <div className={styles.chat_list_and_folders}>
                <div className={styles.folders_container}>
                    <div className={styles.folders}>
                        <div
                            className={`${styles.chats_folder} ${selectedFolder === 'chats' ? styles.active : ''}`}
                            onClick={() => setSelectedFolder('chats')}
                        >Чаты</div>
                        <div
                            className={`${styles.groups_folder} ${selectedFolder === 'groups' ? styles.active : ''}`}
                            onClick={() => setSelectedFolder('groups')}
                        >Группы</div>
                    </div>
                </div>
                <div className={styles.chat_list}>
                    <ul className={styles.result_list}>
                        {searchedUsers.map(user => (
                            <li
                                key={user.id}
                                className={`${styles.result_user} ${selectedChatId === user.id ? styles.active : ''}`}
                                onClick={() => handleChatSelection(user.id, user.username)}
                            >
                                <div className={styles.result_block}>
                                    <div className={styles.result_photo_block}>
                                        <img src={ProfileImg} alt="" />
                                    </div>
                                    <div className={styles.result_txt_block}>
                                        <div className={styles.result_name}>{user.username}</div>
                                        <div className={styles.result_description}></div>
                                    </div>
                                </div>
                            </li>
                        ))}
                    </ul>
                </div>
            </div>
        </aside>
    );
};
