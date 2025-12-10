(() => {
  "use strict";

  // =========================== Init AOS =========================== //
  AOS.init({
    once: true,
    offset: 50,
    disable: "tablet",
  });

  // =========================== Start of Header =========================== //
  // Header indicator position state
  const headerRef = document.querySelector(".header");
  const navRef = headerRef.querySelector(".navbar");
  const linkRef = navRef.querySelectorAll("a");
  const activeLinkRef = navRef.querySelector(".active");
  const navIndicatorRef = headerRef.querySelector(".indicator");
  const hideOnScrollRef = headerRef.querySelectorAll(".hideOnScroll");
  let indicatorPosition = null;

  const navTogglerRef = headerRef.querySelector(".navToggler");

  // Function to set indicator position
  const setIndicatorPosition = (left, width) => {
    indicatorPosition = { left, width };
    navIndicatorRef.style.left = `${indicatorPosition.left}px`;
    navIndicatorRef.style.width = `${indicatorPosition.width}px`;
  };

  // Update indicator position when active link changes
  window.addEventListener("load", () => {
    if (activeLinkRef) {
      setIndicatorPosition(activeLinkRef.offsetLeft, activeLinkRef.offsetWidth);
    }
    setTimeout(() => {
      navIndicatorRef.style.opacity = 1;
      navIndicatorRef.style.transform = "scaleX(1)";
    }, 300);
  });

  // Handle mouse leave event
  const handleLinkMouseLeave = () => {
    if (activeLinkRef) {
      setIndicatorPosition(activeLinkRef.offsetLeft, activeLinkRef.offsetWidth);
    }
  };
  navRef.addEventListener("mouseleave", handleLinkMouseLeave);

  // Handle mouse enter event
  const handleLinkMouseEnter = (event) => {
    const link = event.currentTarget;
    setIndicatorPosition(link.offsetLeft, link.offsetWidth);
  };

  // Handle link click event
  const handleLinkClick = (event) => {
    const link = event.currentTarget;
    activeLinkRef = link;
    setIndicatorPosition(link.offsetLeft, link.offsetWidth);
  };

  linkRef.forEach((link) => {
    link.addEventListener("mouseenter", handleLinkMouseEnter);
    link.addEventListener("click", handleLinkClick);
  });

  // Update Header element position on Scroll
  hideOnScrollRef.forEach((element) => {
    const handleScroll = () => {
      window.scrollY > 50
        ? element.classList.add("scrolled")
        : element.classList.remove("scrolled");
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  });

  // Open-Close Mobile Nav state
  let mobileNavOpen = false;
  const html = document.documentElement;
  const toggleMobileNav = () => {
    mobileNavOpen = !mobileNavOpen;
    if (mobileNavOpen) {
      html.classList.add("overflow-hidden");
      headerRef.classList.add("navOpen");
    } else {
      html.classList.remove("overflow-hidden");
      headerRef.classList.remove("navOpen");
    }
  };
  navTogglerRef.addEventListener("click", toggleMobileNav);

  // Change Header background color on scroll
  window.addEventListener("load", () => {
    const banner = document.querySelector(".banner");
    const bannerScrollHeight = banner ? banner.scrollHeight + 100 : 0;
    const observer = new IntersectionObserver(
      (entry) => {
        window.addEventListener("scroll", () =>
          entry[0].isIntersecting
            ? headerRef.classList.remove("active")
            : headerRef.classList.add("active")
        );

        // Hide Header on scroll down and show on scroll up
        let lastScrollTop = 0;
        const handleScroll = () => {
          const currentScrollTop = document.documentElement.scrollTop;

          currentScrollTop > bannerScrollHeight &&
          currentScrollTop > lastScrollTop
            ? headerRef.classList.add("invisible")
            : headerRef.classList.remove("invisible");

          lastScrollTop = currentScrollTop <= 0 ? 0 : currentScrollTop;
        };
        window.addEventListener("scroll", handleScroll);
        return () => window.removeEventListener("scroll", handleScroll);
      },
      { threshold: [0] }
    );
    if (banner !== null) {
      observer.observe(banner);
    }
  });
  // =========================== End of Header =========================== //

  // =========================== Start of Banner =========================== //
  const handleBannerScroll = () => {
    const bannerTextRef = document.querySelector(".banner .bg-text");
    if (bannerTextRef !== null) {
      const scrollValue = window.scrollY;
      bannerTextRef.style.opacity = (1000 - scrollValue) / 1000;
      bannerTextRef.style.transform = `translateX(-${scrollValue}px)`;
    }
  };
  window.addEventListener("scroll", handleBannerScroll);
  // =========================== End of Banner =========================== //

  // =========================== Start of About Image =========================== //
  window.addEventListener("load", () => {
    const aboutImagesRef = document.querySelector(".about-images");
    if (aboutImagesRef !== null) {
      const allImage = aboutImagesRef.querySelectorAll("img");
      const imageLength = allImage.length;
      const swapImageBtn = document.querySelector(".swap-images-btn");
      const swapImageBtnIcon = swapImageBtn.querySelector("svg");

      // generate random numbers
      const numbers = [];
      const min = -6;
      const max = 6;
      while (numbers.length < imageLength) {
        const randomNumber = Math.floor(Math.random() * (max - min + 1)) + min;
        if (!numbers.includes(randomNumber)) {
          numbers.push(randomNumber);
        }
      }

      // rotate images randomly and set zindex
      setTimeout(() => {
        aboutImagesRef.querySelectorAll("img").forEach((image, index) => {
          image.parentElement.style.transform = `rotate(${numbers[index]}deg)`;
          image.parentElement.style.zIndex = `${numbers[index]}`;
        });
      }, 300);

      // chnage zIndex on click to show image 1 by 1 and rotate button icon
      let currentZIndex = 1;
      let rotateValue = 0;
      const handleClick = () => {
        for (let i = 0; i < imageLength; i++) {
          allImage[i].parentElement.style.zIndex = currentZIndex;
          currentZIndex--;
          if (currentZIndex < -imageLength) {
            currentZIndex = 0;
          }
        }
        swapImageBtnIcon.style.transform = `rotate(${rotateValue + 360}deg)`;
        rotateValue += 360;
      };
      aboutImagesRef.addEventListener("click", handleClick);
      swapImageBtn.addEventListener("click", handleClick);
    }
  });
  // =========================== End of About Image =========================== //

  // =========================== Start of FAQ =========================== //
  const faqItemRef = document.querySelectorAll(".faq-item");
  if (faqItemRef.length !== 0) {
    faqItemRef.forEach((item) => {
      const faqItemHeaderRef = item.querySelector(".faq-item-header");
      const faqItemContentRef = item.querySelector(".faq-item-content");
      const faqItemIconPlus = item.querySelector("svg.inline-block");
      const faqItemIconMinus = item.querySelector("svg.hidden");

      const handleFaqItemClick = () => {
        faqItemContentRef.classList.toggle("hidden");
        faqItemIconPlus.classList.toggle("hidden");
        faqItemIconMinus.classList.toggle("hidden");
      };
      faqItemHeaderRef.addEventListener("click", handleFaqItemClick);
    });
  }
  // =========================== End of FAQ =========================== //

  // =========================== Start of Contact form =========================== //
  const contactFormRef = document.querySelector(".contact-form");
  if (contactFormRef !== null) {
    const submitBtnRef = contactFormRef.querySelector("button[type='submit']");
    const submitBtnTextRef = submitBtnRef.innerHTML;
    const statusRef = contactFormRef.querySelector(".status");
    const emailAddress = "platoltheme@gmail.com";
    const formsubmitURL = `https://formsubmit.co/ajax/${emailAddress}`;

    const formHandler = (e) => {
      e.preventDefault();

      submitBtnRef.innerHTML = "<span>Sending..</span>";

      fetch(formsubmitURL, {
        method: "POST",
        headers: { "Content-type": "application/json" },
        body: JSON.stringify({
          _subject: "Message form Aver Html!",
          name: full_name.value,
          email: email.value,
          message: message.value,
        }),
      })
        .then((response) => response.json())
        .then((data) => {
          statusRef.classList.remove("hidden");
          statusRef.style.color = "#16A34A";
          statusRef.innerHTML = "Submitted Successfully!";

          setTimeout(() => {
            statusRef.classList.add("hidden");
            statusRef.innerHTML = submitBtnTextRef;
          }, 5000);

          e.target.reset();
        })
        .catch((error) => {
          statusRef.classList.remove("hidden");
          statusRef.style.color = "#DC2626";
          statusRef.innerHTML = "Something went wrong!";
        });
    };
    contactFormRef.addEventListener("submit", formHandler);
  }

  // =========================== End of Contact form =========================== //
})();
