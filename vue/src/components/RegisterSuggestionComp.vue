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
                            <p>blablaJa a</p>
                        </h4>
                    </div>
                </div>
            </box>
        </div>

        <div class="box-premium">
            <box class="box-pre">
                <div class="media-content">
                    <div class="content">
                        <div class="myClass animate__animated animate__hoveranimate__bounce"></div>
                        <h2><strong> <i class="fa-solid fa-peace"></i> PREMIUM</strong>
                        </h2>
                        <h4>
                            <p><i class="fa-solid fa-floppy-disk"></i>&nbsp;&nbsp;Save unlimited records to your library</p>
                            <p><i class="fa-solid fa-infinity"></i>&nbsp;&nbsp;Create unlimited collections</p>
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
                        <h2><strong> <i class="fa-solid fa-crown"></i> BASIC</strong>
                        </h2>
                        <h4>
                            <p><i class="fa-solid fa-floppy-disk"></i>&nbsp;&nbsp;Save records to your library</p>
                            <p><i class="fa-solid fa-box"></i>&nbsp;&nbsp;Create up to 25 collections</p>
                            <p><i class="fa-solid fa-record-vinyl"></i>&nbsp;&nbsp;Browse various records</p>
                            <p></p>
                        </h4>
                    </div>
                </div>
            </box>
        </div>
    </div>
</template>

<style scoped>
.box1 {
    display: flex;
    justify-content: center;
    width: 100%;
    align-items: stretch;
    height: 45%;
    padding: 20px;
}

.box-info {
    display: flex;
    width: 950px;
    border: 5px solid rgb(255, 255, 255);
    background-image: linear-gradient(#01B48E, #c5e4dd);
    border-radius: 10px;
    justify-content: center;
    align-items: start;
    padding-right: 36%;
    box-shadow: rgba(0, 0, 0, 0.35) 0px -50px 36px -28px inset;
    transition: transform 0.3s ease;
}

.box-info:hover {
    transition: transform 0.3s ease;
    transform: translateY(-10px);
}

.box-pre {
    padding: 10px;
}

.box-premium {
    display: flex;
    width: 500px;
    border: 5px solid rgb(255, 255, 255);
    background-image: linear-gradient(#D1D301, #f5f698);
    border-radius: 10px;
    justify-content: left;
    transition: transform 0.3s ease;
    text-align: left;
    margin-left: 20px;
    margin-right: 20px;
}

.box-premium:hover {
    transition: transform 0.3s ease;
    transform: translateY(-10px);
}

.box-free {
    display: flex;
    width: 500px;
    border: 5px solid rgb(255, 255, 255);
    background-image: linear-gradient(#1CA2C0, rgb(117, 192, 219));
    border-radius: 10px;
    justify-content: left;
    text-align: left;
    transition: transform 0.3s ease;

}

.box-free:hover {
    transition: transform 0.3s ease;
    transform: translateY(-10px);
}


h4 {
    color: white;
}

.fa-solid {
    color: white;
    padding-top: 15px;
}
</style>