function ChangeTheme() {
    let el = document.getElementById("theme");

    switch(el.getAttribute("data-bs-theme")){
      case "light":
        el.setAttribute("data-bs-theme", "dark");
        setCookie("theme", "dark", 10);
        break;
      case "dark":
        el.setAttribute("data-bs-theme", "light");
        setCookie("theme", "light", 10);
        break;
    }
}

function setCookie(name, value, daysToLive) {
    const date = new Date();
    date.setTime(date.getTime() + (daysToLive * 24 * 60 * 60 * 1000));
    let expires = "expires=" + date.toUTCString();

    document.cookie = `${name}=${value}; ${expires}; SameSite=None; Secure; path=/`;    
}

function getCookie(name) {
    if (document.cookie === "") return null;
    return decodeURIComponent(document.cookie).substring(name.length + 1);
}

function OnInit() {
    if(getCookie("theme") === null ) {
        setCookie("theme", "light", 10);
    }

    let el = document.getElementById("theme");
    let str = getCookie("theme");
    el.setAttribute("data-bs-theme", str);

    if(str === "dark"){
        document.getElementById("darkSwitch").checked = true;
    }
}