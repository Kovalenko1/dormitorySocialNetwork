import styles from './Sidebar.module.scss';
import SearchImg from '../../../assets/img/Search.svg';
import ProfileImg from '../../../assets/img/Person.svg';
import {useEffect, useState} from "react";
import {useDispatch, useSelector} from "react-redux";
import { searchUsers } from '../../../store/slices/searchSlice';
import {debounce} from "lodash";

export const Sidebar = () => {
    const [isFocused, setIsFocused] = useState(false);
    const [selectedFolder, setSelectedFolder] = useState('chats');
    const [query, setQuery] = useState('');

    const dispatch = useDispatch();
    const users = useSelector(state => state.search.list);

    const debouncedSearch = debounce((query) => {
        dispatch(searchUsers(query));
    }, 800);



    return (
        <aside className={styles.chat_sidebar}>
            <div className={styles.search_bar_container}>
                <div className={styles.search_bar}>
                    <div className={styles.search_img_container}>
                        <img className={styles.search_img} src={SearchImg} alt=""/>
                    </div>
                    <input
                        type="text"
                        onFocus={() => setIsFocused(true)}
                        onBlur={() => setIsFocused(false)}
                        onChange={(e) => {
                            debouncedSearch(e.target.value);
                        }}
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
                        {users.map(user => (
                            <li className={styles.result_user}>
                                <div className={styles.result_block}>
                                    <div className={styles.result_photo_block}>
                                        <img src={ProfileImg} alt=""/>
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
}