import React,{useState} from 'react';
import { IoMoonOutline } from "react-icons/io5";
import { GoSun } from "react-icons/go";

const ThemeSwitcher = () => {
    const [theme, setTheme] = useState('light'); // начальная тема - light

    const handleThemeChange = () => {
        setTheme(theme === 'dark' ? 'light' : 'dark');
    };
    React.useEffect(() => {
        document.querySelector('html').setAttribute('data-theme', theme);
      }, [theme]);
    return (
        
        <label className="swap swap-rotate">
          {/* this hidden checkbox controls the state */}
          <input type="checkbox" className="theme-controller" value="synthwave" checked={theme === 'dark'} onChange={handleThemeChange} />
    
          {/* sun icon */}
          <svg className={`swap-off fill-current  cursor-pointer w-6 h-6 ${theme === 'dark' ? 'hidden' : ''}`} viewBox="0 0 16 16">
            <GoSun />
          </svg>
    
          {/* moon icon */}
          <svg className={`swap-on fill-current  cursor-pointer w-6 h-6 ${theme === 'light' ? 'hidden' : ''}`} viewBox="0 0 16 16">
            <IoMoonOutline />
          </svg>
    
        </label>
      );
    };

export default ThemeSwitcher;