function windowScroll() {
  var e = document.getElementById("navbar");
  50 <= document.body.scrollTop || 50 <= document.documentElement.scrollTop
    ? e.classList.add("nav-sticky")
    : e.classList.remove("nav-sticky");
}
window.addEventListener("scroll", function (e) {
  e.preventDefault(), windowScroll();
});
var counter = document.querySelectorAll(".counter-value"),
  speed = 250;
function numberWithCommas(e) {
  return e.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
counter &&
  counter.forEach(function (i) {
    !(function e() {
      var t = +i.getAttribute("data-target"),
        o = +i.innerText,
        n = t / speed;
      n < 1 && (n = 1),
        o < t
          ? ((i.innerText = (o + n).toFixed(0)), setTimeout(e, 1))
          : (i.innerText = numberWithCommas(t)),
        numberWithCommas(i.innerText);
    })();
  });
var swiper = new Swiper(".homeslider", {
    loop: !0,
    effect: "coverflow",
    grabCursor: !0,
    centeredSlides: !0,
    slidesPerView: "2",
    coverflowEffect: {
      rotate: 50,
      stretch: 0,
      depth: 100,
      modifier: 1,
      slideShadows: !1,
      slidesPerView: 2,
    },
    autoplay: { delay: 2500, disableOnInteraction: !1 },
    pagination: { el: ".swiper-pagination" },
    breakpoints: {
      0: { slidesPerView: 1, spaceBetween: 20 },
      576: { slidesPerView: 2 },
      768: { slidesPerView: 4 },
    },
  }),
  swiper = new Swiper(".client-slider", {
    slidesPerView: 1,
    spaceBetween: 30,
    loop: !0,
    pagination: { el: ".swiper-pagination", clickable: !0 },
    navigation: {
      nextEl: ".swiper-button-next",
      prevEl: ".swiper-button-prev",
    },
    autoplay: { delay: 2500, disableOnInteraction: !1 },
    breakpoints: { 1024: { slidesPerView: 2, spaceBetween: 20 } },
  }),
  mybutton =
    (setTimeout(function () {
      var e = document.querySelectorAll(".submenu-item li a");
      e &&
        e.forEach(function (e) {
          var t = window.location.href.split(/[?#]/)[0];
          e.href == t && e.classList.add("active");
        });
    }, 0),
    document.getElementById("back-to-top"));
function scrollFunction() {
  50 < document.body.scrollTop || 50 < document.documentElement.scrollTop
    ? ((mybutton.style.opacity = "1"), (mybutton.style.bottom = "20px"))
    : ((mybutton.style.opacity = "0"), (mybutton.style.bottom = "50px"));
}
function topFunction() {
  (document.body.scrollTop = 0), (document.documentElement.scrollTop = 0);
}
window.onscroll = function () {
  scrollFunction();
};
swiper = new Swiper(".mySwiper", {
  spaceBetween: 30,
  loop: !0,
  autoplay: { delay: 2500, disableOnInteraction: !1 },
});
