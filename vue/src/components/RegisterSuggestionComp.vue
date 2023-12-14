<template>
    <div class="box1">
        <div class="box-info">
            <box class="box-pre">
                <div class="media-content">
                    <div class="content">
                        <div class="myClass animate__animated animate__hoveranimate__bounce"></div>
                        <h2 style="color: #363636"><strong> <i class="fa-solid fa-bolt" style="color: #363636"></i> ABOUT JACA</strong>
                        </h2>
                        <h4>
                            <p style="color: #363636"><i class="fa-solid fa-info" style="color: #363636"></i>&nbsp;&nbsp;JACA RECORDS began in the classroom by four .NET
                                bootcamp students. This final capstone
                                assignment turned into a passion project which dives into an unparalleled audio adventure as
                                that seamlessly connects music enthusiasts with their favorite artists across genres. We
                                go beyond mainstream hits, ensuring a unique and tailored music journey for every user and
                                strive to create a friendly user atmosphere for our users and music lovers alike.
                                Welcome, to JACA RECORDS. (WE ARE VINYL ONLY - hipsters only)
                            </p>
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
                        <h2 style="color: #363636"><strong> <i class="fa-solid fa-peace" style="color: #363636"></i> PREMIUM</strong>
                        </h2>
                        <h4 style="color: #363636">
                            <p style="color: #363636"><i class="fa-solid fa-floppy-disk" style="color: #363636"></i>&nbsp;&nbsp;Save unlimited records to your library</p>
                            <p style="color: #363636"><i class="fa-solid fa-infinity" style="color: #363636"></i>&nbsp;&nbsp;Create unlimited collections</p>
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
                        <h2 style="color: #363636"><strong> <i class="fa-solid fa-crown" style="color: #363636"></i> BASIC</strong>
                        </h2>
                        <h4 style="color: #363636">
                            <p style="color: #363636"><i class="fa-solid fa-floppy-disk" style="color: #363636"></i>&nbsp;&nbsp;Save records to your library</p>
                            <p style="color: #363636"><i class="fa-solid fa-box" style="color: #363636"></i>&nbsp;&nbsp;Create up to 25 collections</p>
                            <p style="color: #363636"><i class="fa-solid fa-record-vinyl" style="color: #363636"></i>&nbsp;&nbsp;Browse various records</p>
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
    background-image: linear-gradient(#07B6A0, #c5e4dd);
    border-radius: 10px;
    justify-content: center;
    align-items: start;
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
    background-image: linear-gradient(#EEE810, #f5f6c9);
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
    background-image: linear-gradient(#08A2D9, rgb(176, 221, 237));
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