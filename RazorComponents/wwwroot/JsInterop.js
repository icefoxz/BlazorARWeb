// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.
export function openInNewTab(url) {
    window.open(url, "_blank");
}
export function showPrompt(message) {
  return prompt(message, 'Type anything here');
}
export function openImageInNewTab(base64Image) {
    var image = new Image();
    image.src = base64Image;
    var w = window.open("");
    w.document.write(image.outerHTML);
}
export function getLocation() {
    return new Promise((resolve, reject) => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                position => resolve({ lat: position.coords.latitude, lon: position.coords.longitude }),
                err => reject(err)
            );
        } else {
            reject("Geolocation is not supported by this browser.");
        }
    });
}
export function goToTop(className) {
    const contentElement = document.getElementsByClassName(className)[0];
    if (contentElement) {
        contentElement.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    } else {
        console.error('Element with class name "' + className + '" not found.');
    }
}

export function goToTopById(elementId) {
    var element = getElementById(elementId);
    if (element) {
        element.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    } else {
        console.error('Element with ID "' + elementId + '" not found.');
    }
}

export function goToBottomById(elementId) {
    var element = getElementById(elementId);
    if (element) {
        element.scrollTo({
            top: element.scrollHeight,
            behavior: 'smooth'
        });
    } else {
        console.error('Element with ID "' + elementId + '" not found.');
    }
}

export function goToBottom(className) {
    const element = document.getElementsByClassName(className)[0];
    if (element) {
        element.scrollTo({
            top: element.scrollHeight,
            behavior: 'smooth'
        });
    } else {
        console.error('Element with class name "' + className + '" not found.');
    }
}

export function alignToElementId(targetId, allignElementId, elementId) {
    var targetElement = getElementById(targetId);
    var alignElement = getElementById(allignElementId);
    var scrollElement = getElementById(elementId);

    if (targetElement && alignElement && scrollElement) {
        // 获取评论和控制台元素的位置和高度
        var commentRect = targetElement.getBoundingClientRect();
        var consoleRect = alignElement.getBoundingClientRect();

        // 计算需要滚动的偏移量
        var scrollOffset = commentRect.bottom - consoleRect.top + scrollElement.scrollTop;

        // 确保滚动偏移量为正数
        scrollOffset = Math.max(0, scrollOffset);

        // 平滑滚动到计算出的偏移量
        scrollElement.scrollTo({
            top: scrollOffset,
            behavior: 'smooth'
        });
    }
}

export function scrollIntoView (elementId) {
    var element = getElementById(elementId);
    if (element) {
        element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
};

export function previewImage(inputFileElementId, imageId, convertMb, targetSize, dotNetHelper) {
    const fileInput = getElementById(inputFileElementId);
    const imageElement = getElementById(imageId);
    if (fileInput && fileInput.files && fileInput.files.length > 0) {
        const file = fileInput.files[0];
        const reader = new FileReader();

        reader.onload = function (event) {
            // 创建一个临时图片来读取尺寸
            const tempImage = new Image();
            tempImage.onload = function () {
                // 获取图片的原始宽度和高度
                const originalWidth = tempImage.width;
                const originalHeight = tempImage.height;

                // 计算目标尺寸，保持原始宽高比
                const aspectRatio = originalWidth / originalHeight;
                let maxWidth, maxHeight;

                if (aspectRatio > 1) { // 横向图片
                    maxWidth = targetSize;
                    maxHeight = targetSize / aspectRatio;
                } else { // 纵向图片
                    maxHeight = targetSize;
                    maxWidth = targetSize * aspectRatio;
                }

                // 使用 Compressor.js 进行图片压缩
                new Compressor(file, {
                    maxWidth: maxWidth,
                    maxHeight: maxHeight,
                    quality: 0.8, // 压缩质量
                    convertSize: convertMb * 1024 * 1024, // 转换为JPEG的阈值
                    success: function (compressedResult) {
                        // 使用 FileReader 读取压缩后的 Blob
                        const compressionReader = new FileReader();
                        compressionReader.onloadend = function () {
                            // 更新图像元素并回调到 Blazor
                            imageElement.src = compressionReader.result;
                            dotNetHelper.invokeMethodAsync('PreviewImageCallback', compressionReader.result)
                                .catch(error => console.error('Error calling back .NET.', error));
                        };
                        compressionReader.readAsDataURL(compressedResult);
                    },
                    error: function (err) {
                        console.error('Error compressing image:', err.message);
                    }
                });
            };
            tempImage.src = event.target.result;
        };
        reader.onerror = error => console.error('Error reading file:', error);
        reader.readAsDataURL(file);
    }
}

export function getElementById(elementId) {
    var element = document.getElementById(elementId);
    return element;
};
export function getElementValueById(elementId) {
    return getElementById(elementId).value;   
};                        

export function getScreenSizeLabel() {
    const width = window.innerWidth;
    if (width < 600) return 'xs';
    if (width < 960) return 'sm';
    if (width < 1280) return 'md';
    if (width < 1920) return 'lg';
    if (width < 2560) return 'xl';
    return 'xxl';
}

// listen for page resize
let resizeListener = null;
let cancelResizeListening = false;

export function onResize(dotNetHelper) {
    cancelResizeListening = false;
    if (resizeListener === null) {
        let lastLabel = getScreenSizeLabel();
        dotNetHelper.invokeMethodAsync('UpdateScreenSize', lastLabel);

        resizeListener = () => {
            if (cancelResizeListening) return;
            const newLabel = getScreenSizeLabel();
            if (newLabel !== lastLabel) {
                lastLabel = newLabel;
                dotNetHelper.invokeMethodAsync('UpdateScreenSize', newLabel);
            }
        };

        window.addEventListener('resize', resizeListener);
    }
}

export function cancelResize() {
    if (resizeListener !== null) {
        cancelResizeListening = true;
        window.removeEventListener('resize', resizeListener);
        resizeListener = null;
    }
}