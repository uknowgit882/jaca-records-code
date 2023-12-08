<template>
    
    <div class="box1">
        <div class="box-info">
            <box class="box-pre">
                <div class="media-content">
                    <div class="content">
                        <h2><strong> <i class="fa-solid fa-bolt"></i>About Jaca Records:</strong>
                        </h2>
                        <h4>
                            blablabla
                        </h4>
                    </div>
                </div>
            </box>
        </div>

        <div class="box-free">
            <box class="box-pre">
                <div class="media-content">
                    <div class="content">
                        <h2><strong> <i class="fa-solid fa-peace"></i>Free Users</strong>
                        </h2>
                        <h3>$Free</h3>
                        <h4>
                            blablabla
                        </h4>
                    </div>
                </div>
                <a href="/register">
                <button class="button is-success">Sign up</button>
            </a>
            </box>
        </div>


        <div class="box-premium">
            <box class="box-pre">
                <div class="media-content">
                    <div class="content">
                        <h2><strong> <i class="fa-solid fa-crown"></i> Premium Users</strong>
                        </h2>
                        <h3>$5000</h3>
                        <h4>
                            blablabla
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
    height: 500px;
    border: 5px solid rgb(255, 255, 255);
    background: rgba(168, 232, 238, 0.826);
    border-radius: 10px;
    margin-right: 20px;
    margin-left: 20px
}

.box-premium {
    display: flex;
    width: 500px;
    height: 500px;
    border: 5px solid rgb(255, 255, 255);
    background: rgba(228, 231, 97, 0.826);
    border-radius: 10px;
    justify-content: center;
    align-items: start;
}

.box1 {
    display: flex;
    justify-content:center;
    width: 95%;
    align-items: end;
    height: 90vh;
}
.box-info{
    display: flex;
    width: 500px;
    height: 500px;
    border: 5px solid rgb(255, 255, 255);
    background: rgba(231, 97, 155, 0.719);
    border-radius: 10px;
    justify-content: center;
    align-items: start;
    
}
</style>