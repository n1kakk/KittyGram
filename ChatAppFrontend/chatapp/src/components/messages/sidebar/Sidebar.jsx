import React, {useState} from 'react'
import SearchInput from './SearchInput';
import Conversations from './Conversations';
import LogoutButton from './LogoutButton';
import ThemeSwitcher from '../../../pages/home/ThemeSwitcher';
import { IoIosMenu } from "react-icons/io";

// export default Sidebar;
const Sidebar = () => {
  const [isOpen, setIsOpen] = useState(true); // Состояние, отслеживающее открытие и закрытие боковой панели

  // Функция для переключения состояния открытия/закрытия боковой панели
  const toggleSidebar = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div className={`border-r border-slate-300 p-4 flex flex-col ${isOpen ? 'w-65' : 'w-50'}`}>
    {/* <div className={`border-r border-slate-500 px-3 py-3 mb-2.5 p-3 flex flex-col ${isOpen ? 'w-65' : 'w-0'}`} > */}

          {/* <button onClick={toggleSidebar}>
            {isOpen ? 'Close' : <LuMenu className='fill-current w-6 h-6 cursor-pointer '/>
          
          }
          </button> */}
          <div className=' px-3 py-1 mb-2.5'>
          <button onClick={toggleSidebar}>
            {isOpen ? 'Close' : <IoIosMenu   className={`${isOpen ? '' : ''} flex flex-col absolute left-4 top-4 fill-current w-6 h-6 cursor-pointer `} />}
          </button>
          </div>

          {!isOpen && (
            <div className="flex flex-col absolute bottom-4 left-4">
              <ThemeSwitcher />
              <div className="my-2"></div>
              <LogoutButton />
            </div>

          )}
          


      {/* Весь контент боковой панели */}

      {isOpen && (
        <>
          
          <SearchInput />
          <div className='divider px-3'></div>
          <Conversations />
          <div className='divider px-3'></div>
          <div className="flex justify-between items-center">
            <LogoutButton />
            <ThemeSwitcher />
          </div>
        </>
      )}

    </div>
  );
};

export default Sidebar;