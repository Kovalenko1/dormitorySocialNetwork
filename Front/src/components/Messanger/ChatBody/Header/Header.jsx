import style from './Header.module.scss'
import PersonImg from '../../../../assets/img/Person.svg'
import PhoneImg from '../../../../assets/img/Phone.svg'
import CameraImg from '../../../../assets/img/Camera.svg'

export const Header = () => {
    return (
        <header>
            <div className={style.friend_profile}>
                <img className={style.friend_img} src={PersonImg} alt=""/>
                <div className={style.friend_profile_subscription}>
                            <span className={style.friend_name}>
                                Name
                            </span>
                    <span className={style.active}>
                                Active
                            </span>
                </div>
            </div>

            <div className={style.communicate_bar}>
                <img className={style.phone_img} src={PhoneImg} alt=""/>
                <img src={CameraImg} alt=""/>
            </div>
        </header>
    );
};