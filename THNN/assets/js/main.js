
// Slider
const imgMain = document.querySelector('.slider__img-main img');
const listImg = document.querySelectorAll('.slider__main-bot img');
const nextPtn = document.querySelector('.next');
const prevPtn = document.querySelector('.prev');

const listDot = document.querySelectorAll(".slider__main-bot label");

function changeImg(index) {
    currentIndex = index
    document.querySelector(".slider__main-bot .active").classList.remove("active");
    listImg[index].parentElement.classList.add("active");
    imgMain.src = listImg[index].getAttribute('src');
}



let currentIndex = 0;
listImg.forEach((img, index) => {
    console.log(img);
    img.addEventListener("click", (e) => {
        changeImg(index);
    });
});

prevPtn.addEventListener("click", (e) => {
    if (currentIndex == 0) {
        currentIndex = listImg.length;
    }
    currentIndex = currentIndex - 1
    changeImg(currentIndex)
})

listDot.forEach((dots, index) => {
    dots.addEventListener("click", (e) => {
        changeImg(index)
    })
})

nextPtn.addEventListener("click", (e) => {
    console.log(currentIndex);
    if (currentIndex == listImg.length - 1) {
        currentIndex = 0;
        changeImg(currentIndex)
    }
    else {
        currentIndex = currentIndex + 1
        changeImg(currentIndex)
    }
})

setInterval(() => {
    if (currentIndex == listImg.length - 1) {
        currentIndex = 0;
        changeImg(currentIndex)
    }
    else {
        currentIndex = currentIndex + 1
        changeImg(currentIndex)
    }
}, 7500)

// active side bar
/*
const listItem = document.querySelectorAll(".side_transition-item-top>a")
const listDetailItem = document.querySelectorAll(".side_transition-item-top .side_transition-detail .side_transition-detail-item");


listItem.forEach((item) => {
    item.addEventListener("click", () => {
        // console.log(item.parentElement.children[1].children);
        const isActive = item.parentElement.classList.contains("active");

        if (isActive) {
            item.parentElement.classList.remove("active");
        } else {

            listItem.forEach((otherItem) => {
                otherItem.parentElement.classList.remove("active");
            });
            item.parentElement.classList.add("active");
        }
    });
});

listDetailItem.forEach((item) => {
    item.addEventListener("click", () => {
        console.log(item);
    })
})*/

