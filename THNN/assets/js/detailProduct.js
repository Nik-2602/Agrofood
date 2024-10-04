//img product choice
const dpMainImg = document.querySelector(".dp_left-img-main");
const btnLeft = document.querySelector(".left");
const btnRight = document.querySelector(".right");
const listDPImg = document.querySelectorAll(".dp_left-item-img img");




let currentIndexDP =0;

function changeImg2(index) {
  currentIndexDP = index
document.querySelector(".dp_left-item-img.active").classList.remove("active");
listDPImg[index].parentElement.classList.add("active");
dpMainImg.src = listDPImg[index].getAttribute('src');
}

btnLeft.addEventListener("click", (e) => {
  if (currentIndexDP == 0) {
    currentIndexDP = listDPImg.length ;
  }
  currentIndexDP = currentIndexDP -1
  changeImg2(currentIndexDP)
})

btnRight.addEventListener("click", (e) => {
  if (currentIndexDP == listDPImg.length-1) {
    currentIndexDP  = 0;
      changeImg2(currentIndexDP) 
  }
  else {
    currentIndexDP = currentIndexDP + 1
  changeImg2(currentIndexDP)
  } 
})

listDPImg.forEach((item,index)=> {
  item.addEventListener("click", (e) =>{
    changeImg2(index)
  })
})






//click review
/*const listClickReview = document.querySelectorAll(".review_prodcut-user-list-desc.review");
const modal = document.querySelector(".modal__review");
const modalContainer = document.querySelector(".modal__container");
const btnClose = document.querySelector(".modal__close");

listClickReview.forEach((item)=> {
  item.addEventListener("click", (e) =>{
      modal.classList.add("open");
  })
})

btnClose.addEventListener("click",(e)=> {
  modal.classList.remove("open");
})

modal.addEventListener("click",(e)=> {
  modal.classList.remove("open");
})

modalContainer.addEventListener("click",(e)=>{
  e.stopPropagation();
})

const listStartFromModal = document.querySelectorAll(".review_prodcut-user-list-desc.in-modal");
listStartFromModal.forEach((item)=>{
  item.addEventListener("click",(e)=>{
    // if (e.target == document.querySelector(".review_prodcut-user-list-desc.in-modal i")) {
    //   console.log(1);
    // }

    if(item.firstElementChild.matches(".far.fa-star.start_1.active")){
      item.firstElementChild.classList.remove("active");
      item.children[1].classList.add("active");
    }
    else {
      listStartFromModal.forEach((otherItem)=> {
        otherItem.children[1].classList.remove("active");
      })
      item.firstElementChild.classList.add("active");
    }
    // console.log(item.children[1]);
    
  });
})*/


//active side bar
// let currentChooseSideBar;
const listItemSB = document.querySelectorAll(".side_transition-item-top>a")
console.log(listItemSB);
// const listDetailItemSB = document.querySelectorAll(".side_transition-item-top .side_transition-detail .side_transition-detail-item");
// console.log(listDetailItemSB);

listItemSB.forEach((item) => {
  item.addEventListener("click", () => {
    const isActive = item.parentElement.classList.contains("active");

    if (isActive) {
      item.parentElement.classList.remove("active");
    } else {

      listItemSB.forEach((otherItem) => {
        otherItem.parentElement.classList.remove("active");
      });
      item.parentElement.classList.add("active");
    }
  });
});

