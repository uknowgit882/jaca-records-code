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
    background-image: linear-gradient(#1CA2C0, rgb(117, 192, 219));
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
    background-image: linear-gradient(#D1D301, #f5f698);
    border-radius: 10px;
    margin-right: -80px;
    justify-content: center;
    align-items: start;
;

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
    width: 960px;
    height: 460px;
    border: 5px solid rgb(255, 255, 255);
    background-image: linear-gradient(#01B48E, #c5e4dd);
    border-radius: 10px;
    justify-content: center;
    align-items: start;
    padding-right: 36%;
    box-shadow: rgba(0, 0, 0, 0.35) 0px -50px 36px -28px inset;
    margin-left: 30px;
}

.fa-solid {
    color: white;
    padding-top: 15px;
}

.button {
 --color: #000000;
 font-family: inherit;
 display: inline-block;
 width: 20em;
 height: 2.6em;
 line-height: 1.5em;
 margin: 20px;
 position: relative;
 overflow: hidden;
 border: 2px solid var(--color);
 transition: color .5s;
 z-index: 1;
 font-size: 17px;
 border-radius: 6px;
 font-weight: 500;
 color: var(--color);
 
}

.button:before {
 content: "";
 position: absolute;
 z-index: -1;
 background: var(--color);
 height: 115px;
 width: 500px;
 border-radius: 50%;
}

.button:hover {
 color: #fff;
}

.button:before {
 top: 100%;
 left: 100%;
 transition: all .7s;
}

.button:hover:before {
 top: -30px;
 left: -30px;
}

.button:active:before {
 background: #3a0ca3;
 transition: background 0s;
}
</style>