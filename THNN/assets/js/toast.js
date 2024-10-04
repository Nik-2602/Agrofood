function toast({ title = "", message = "", type = "", duration }) {
    const main = document.getElementById("toast");
    if (main) {
        const toast = document.createElement("div");
        const icons = {
            success: "fas fa-check-circle",
            error: "fas fa-exclamation-triangle",
            warning: "fas fa-exclamation",
            info: "fas fa-info-circle",
        };

        const autoRemoveId = setTimeout(function () {
            main.removeChild(toast);
        }, duration + 1000);

        toast.onclick = function (e) {
            if (e.target.closest(".toast__close")) {
                main.removeChild(toast);
                clearTimeout(autoRemoveId);
            }
        };

        const delay = (duration / 1000).toFixed(2);
        const icon = icons[type];
        toast.classList.add("toastRB", `toast--${type}`);
        toast.style.animation = `slideInLeft ease 0.3s, fadeOut linear 1s ${duration}s forwards`;
        toast.innerHTML = ` <div class="toast__icon">
        <i class="${icon}"></i>
      </div>

      <div class="toast__content">
        <h3 class="toast__title">${title}</h3>
        <p class="toast__msg">${message}</p>
      </div>

      <div class="toast__close">
        <i class="fas fa-times"></i>
      </div>`;
        main.appendChild(toast);
    }
}

function showErrorToast() {
    toast({
        title: "Thất bại",
        message: "Đã có lỗi xảy ra",
        type: "error",
        duration: 3000,
    });
}

function showSuccessToast() {
    toast({
        title: "Thành công",
        message: "Chúc mừng bạn đã đăng ký thành cộng",
        type: "success",
        duration: 3000,
    });
}



