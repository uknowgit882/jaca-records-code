<template>
    <div class="box1">
        <div class="box-info">
            <box class="box-pre">
                <div class="media-content">
                    <div class="content">
                        <div class="myClass animate__animated animate__hoveranimate__bounce"></div>
                        <h2><strong> <i class="fa-solid fa-bolt"></i> ABOUT JACA</strong>
                        </h2>
                        <h4>
                            <p>blablaJacablabla</p>
                        </h4>
                    </div>
                </div>
            </box>
        </div>

        <div class="box-free">
            <box class="box-pre">
                <div class="media-content">
                    <div class="content">
                        <div class="myClass animate__animated animate__hoveranimate__bounce"></div>
                        <h2><strong> <i class="fa-solid fa-peace"></i> BASIC</strong>
                        </h2>
                        <h3><strong>Free</strong></h3>
                        <h4>
                            <p><i class="fa-solid fa-record-vinyl"></i>&nbsp;&nbsp;Browse various records</p>
                            <p><i class="fa-solid fa-floppy-disk"></i>&nbsp;&nbsp;Save records to your library</p>
                            <p><i class="fa-solid fa-box"></i>&nbsp;&nbsp;Create up to 25 collections</p>
                            <p><i class="fa-solid fa-users"></i>&nbsp;&nbsp;Add friends and view their records</p>
                        </h4>
                    </div>
                </div>
                <a href="/register">
                    <button class="button is-success">Sign Up</button>
                </a>
            </box>
        </div>


        <div class="box-premium">
            <box class="box-pre">
                <div class="media-content">
                    <div class="content">
                        <div class="myClass animate__animated animate__hoveranimate__bounce"></div>
                        <h2><strong> <i class="fa-solid fa-crown"></i> PREMIUM</strong>
                        </h2>
                        <h3><strong>$9.99 /</strong> month</h3>
                        <h4>
                            <p><i class="fa-solid fa-infinity"></i>&nbsp;&nbsp;Create unlimited collections</p>
                            <p></p>
                        </h4>
                    </div>
                </div>
                <a href="/register">
                    <button class="button is-success">Sign up</button>
                </a>
            </box>
        </div>
    </div>
</template>

<style scoped>
.box-free {
    display: flex;
    width: 500px;
    height: 460px;
    border: 5px solid rgb(255, 255, 255);
    background-image: linear-gradient(rgb(11, 122, 225), rgb(117, 192, 219));
    border-radius: 10px;
    margin-right: 20px;
    margin-left: 20px;
    justify-content: center;

}

.content {
    height: 370px;
    transition: transform 0.3s ease;
}

.content:hover {
    transform: translateY(-10px);
}

h4 {
    color: white;
}

.box-premium {
    display: flex;
    width: 500px;
    height: 460px;
    border: 5px solid rgb(255, 255, 255);
    background-image: linear-gradient(rgb(246, 227, 83), rgb(230, 191, 146));
    border-radius: 10px;
    margin-right: -120px;
    justify-content: center;
    align-items: start;

}


.box1 {
    display: flex;
    justify-content: center;
    width: 95%;
    align-items: end;
    height: 50vh;


}

.box-info {
    display: flex;
    width: 700px;
    height: 460px;
    border: 5px solid rgb(255, 255, 255);
    background-image: linear-gradient(rgb(218, 50, 87), rgb(236, 188, 201));
    border-radius: 10px;
    justify-content: center;
    align-items: start;
    padding-right: 23%;
    box-shadow: rgba(0, 0, 0, 0.35) 0px -50px 36px -28px inset;
}

.fa-solid {
    color: white;
    padding-top: 15px;
}

.button {
    text-decoration: none;
    position: relative;
    border: none;
    font-size: 14px;
    font-family: inherit;
    color: #fff;
    width: 30em;
    height: 3em;
    line-height: 2em;
    text-align: center;
    background: linear-gradient(90deg, #03a9f4, #f441a5, #ffeb3b, #03a9f4);
    background-size: 300%;
    border-radius: 30px;
    z-index: 1;
}

.button:hover {
    animation: ani 8s linear infinite;
    border: none;

}

@keyframes ani {
    0% {
        background-position: 0%;
    }

    100% {
        background-position: 400%;
    }
}

.button:before {
    content: '';
    position: absolute;
    top: -5px;
    left: -5px;
    right: -5px;
    bottom: -5px;
    z-index: -1;
    background: linear-gradient(90deg, #03a9f4, #f441a5, #ffeb3b, #03a9f4);
    background-size: 400%;
    border-radius: 35px;
    transition: 1s;

}

.button:hover::before {
    filter: blur(20px);
}

.button:active {
    background: linear-gradient(32deg, #03a9f4, #f441a5, #ffeb3b, #03a9f4);
}
</style>