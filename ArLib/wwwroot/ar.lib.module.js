// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.
const arSystemName = 'mindar-image-system';
function getArSystem() {
    const sceneEl = document.querySelector('a-scene');
    let arSystem = sceneEl.systems[arSystemName];
    return arSystem;
}
export function initAR(blazor) {
    ensureAFrameworkInit(() => {
        CheckMindAR(() => {
            blazor.invokeMethodAsync('OnScriptsLoaded');
        });
    });
}
function ensureAFrameworkInit(callback) {
    // Check if A-Frame is loaded
    if (typeof AFRAME === 'undefined') {
        loadScript('https://aframe.io/releases/1.6.0/aframe.min.js', () => {
            // A-Frame loaded, now check MindAR
            callback()
        });
    } else {
        // A-Frame is already loaded, check MindAR
        callback();
    }
}
function CheckMindAR(callback) {
    if (typeof MindAR === 'undefined') {
        loadScript('https://cdn.jsdelivr.net/npm/mind-ar@1.2.5/dist/mindar-image-aframe.prod.js', () => {
            // MindAR loaded
            callback();
        });
    } else {
        // MindAR is already loaded
        callback();
    }
}

function loadScript(src, onload) {
    const script = document.createElement('script');
    script.src = src;
    script.onload = onload;
    script.onerror = () => console.error(`Failed to load script: ${src}`);
    document.head.appendChild(script);
}
export function startAR() {
    ensureAFrameworkInit(() => {
        getArSystem().start();       
    });
}
//arSystem.stop(); // stop the engine
export function stopAR() {   
    let arSystem = getArSystem();
    arSystem.stop();
};
//arSystem.pause(keepVideo = false); // pause the engine. It has an optional parameter. if true, then ar will stop, but camera feed will keep
//arSystem.unpause(); // unpause
export function restartAR() {
    stopAR();
    startAR();
}
export function regClickable(id, dotNetHelper) {
    const element = document.querySelector(`#${id}`);

    if (element) {
        // 确保元素是 A-Frame 的实体
        if (element.tagName.startsWith('A-')) {
            // 动态添加 'clickable' 类
            element.classList.add('clickable');

            // 注册 A-Frame 的点击事件，通过 raycaster 捕获
            element.addEventListener('click', event => {
                //console.log(`Element with id ${id} was clicked.`);
                dotNetHelper.invokeMethodAsync('OnClick');
            });
        } else {
            console.log('This is not an A-Frame element');
        }
    } else {
        console.error(`Element with id ${id} not found`);
    }
}
export function videoPlay(id) {
    let element = document.querySelector(`#${id}`);
    if (element) {
        element.play();
        return getVideoStatus(element);
    } else {
        console.error(`Element with id ${id} not found`);
        return null;
    }
}
export function videoPause(id) {
    let element = document.querySelector(`#${id}`);
    if (element) {
        element.pause();
        return getVideoStatus(element);
    } else {
        console.error(`Element with id ${id} not found`);
        return null;
    }
}
export function aSoundPause(id) {
    let element = document.querySelector(`#${id}`);
    if (element) {
        element.pauseSound();
    } else {
        console.error(`Element with id ${id} not found`);
    }
}
export function aSoundStop(id) {
    let element = document.querySelector(`#${id}`);
    if (element) {
        element.stopSound();
    } else {
        console.error(`Element with id ${id} not found`);
    }
}
export function aSoundPlay(id) {
    let element = document.querySelector(`#${id}`);
    if (element) {
        element.playSound();
    } else {
        console.error(`Element with id ${id} not found`);
    }
}
export function soundPlay(id) {
    let element = document.getElementById(id);
    if (element) {
        element.play();
    } else {
        console.error(`Element with id ${id} not found`);
    }
}
export function soundStop(id) {
    let element = document.getElementById(id);
    if (element) {
        element.pause();
        element.currentTime = 0;
    } else {
        console.error(`Element with id ${id} not found`);
    }
}
function getVideoStatus(video) {
    return {
        paused: video.paused,
        currentTime: video.currentTime,
        ended: video.ended
    };
}
export function showPrompt(message) {
  return prompt(message, 'Type anything here');
}