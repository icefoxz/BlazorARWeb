// mindar-compiler.js
import { C as o, a as r } from "./controller-mGt1s8dJ.js";
import { U as i } from "./ui-fBadYuor.js";
window.MINDAR || (window.MINDAR = {});
window.MINDAR.IMAGE = {
    Controller: o,
    Compiler: r,
    UI: i
};
export {
    r as Compiler,
    o as Controller,
    i as UI
};
function ensureMindARCompilerScript(callback) {
    // 检查 MINDAR 是否已加载
    if (typeof MINDAR === 'undefined') {
        // MINDAR 未定义，需要加载 mindar.prod.js
        loadScript('_content/ArLib/mindar-image.prod.js', () => {
            console.log('mindar.prod.js 已加载');
            // 检查 MINDAR 是否已定义
            if (typeof MINDAR !== 'undefined') {
                // MINDAR 已定义，执行回调
                callback();
            } else {
                console.error('MINDAR 未定义，加载 mindar.prod.js 失败');
            }
        });       
    } else {
        // MINDAR 已加载，直接执行回调
        callback();
    }
}
function loadScript(src, onload) {
    console.log(`正在加载脚本：${src}`);
    const script = document.createElement('script');
    script.src = src;
    script.onload = () => {
        console.log(`脚本已加载：${src}`);
        if (onload) onload();
    };
    script.onerror = () => {
        console.error(`无法加载脚本：${src}`);
    };
    document.head.appendChild(script);
}

export async function generateMindFile(elementRef) {
    return new Promise(async (resolve, reject) => {
        //try {
            //ensureMindARCompilerScript(async () => {
                // 检查 MINDAR 是否已定义
                if (typeof MINDAR !== 'undefined') {
                    // 在确保 MINDAR 已加载后创建编译器实例
                    const compiler = new MINDAR.IMAGE.Compiler();
                    const images = [];
                    var fileList = elementRef.files;

                    // 加载用户选择的图像文件
                    for (let i = 0; i < fileList.length; i++) {
                        const file = fileList[i];
                        const image = await loadImage(file);
                        images.push(image);
                    }

                    // 编译图像，生成目标数据
                    await compiler.compileImageTargets(images, (progress) => {
                        console.log('编译进度:', progress);
                        // 可以在这里更新进度显示
                    });

                    // 导出 .mind 文件
                    const exportedBuffer = await compiler.exportData();
                    resolve(exportedBuffer); // 成功返回数据
                } else {
                    console.error('MINDAR.Compiler 未正确定义');
                    reject(new Error('MINDAR.Compiler 未正确定义'));
                }
        //    });
        //} catch (error) {
        //    console.error('生成 .mind 文件时出错:', error);
        //    reject(error); // 异常时返回错误
        //}
    });
}

export async function generateMindFileWithProgress(elementRef, blazor) {
    return new Promise(async (resolve, reject) => {
        if (typeof MINDAR !== 'undefined') {
            // 创建编译器实例
            const compiler = new MINDAR.IMAGE.Compiler();
            const images = [];
            var fileList = elementRef.files;

            // 加载用户选择的图像文件
            for (let i = 0; i < fileList.length; i++) {
                const file = fileList[i];
                const image = await loadImage(file);
                images.push(image);
            }

            // 编译图像，生成目标数据
            await compiler.compileImageTargets(images, async (progress) => {
                console.log('编译进度:', progress);
                // 使用 Blazor 的 JS 互操作方法更新进度
                await blazor.invokeMethodAsync('UpdateProgress', progress);
            });

            // 导出 .mind 文件
            const exportedBuffer = await compiler.exportData();
            resolve(exportedBuffer); // 成功返回数据
        } else {
            console.error('MINDAR.Compiler 未正确定义');
            reject(new Error('MINDAR.Compiler 未正确定义'));
        }
    });
}

// 获取 <input type="file"> 选择的文件数量
export function getFileCount(element) {
    return element.files.length;
}

// 加载图像文件为 Image 对象
function loadImage(file) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        img.onload = () => resolve(img);
        img.onerror = reject;
        img.src = URL.createObjectURL(file);
    });
}

// 提供下载功能
export function downloadMindFile(dataUrl) {
    const aLink = document.createElement('a');   
    aLink.href = dataUrl;
    aLink.download = 'targets.mind';
    document.body.appendChild(aLink);
    aLink.click();
    document.body.removeChild(aLink);
};

// 获取 <input type="file"> 选择的文件列表
window.getSelectedFiles = (element) => {
    return element.files;
};

export function initializeImagePreview() {
    // 获取 img 标签和文件输入
    const imgElement = document.getElementById('previewImage');
    const fileInput = document.getElementById('imageInput');

    // 添加文件选择事件监听器
    fileInput.addEventListener('change', (event) => {
        const file = event.target.files[0];
        if (file) {
            // 创建 FileReader 来读取本地文件
            const reader = new FileReader();
            reader.onload = function (e) {
                // 将读取的图像数据设置为 img 标签的 src
                imgElement.src = e.target.result;
            };
            reader.readAsDataURL(file);
        }
    });
}