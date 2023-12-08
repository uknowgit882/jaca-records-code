<template>
    <div class="box1">
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
    width: 300px;
    height: 200px;
    border: 5px solid rgb(255, 255, 255);
    background: rgba(168, 232, 238, 0.826);
    border-radius: 10px;
    display: flex;
    justify-content: center;
    align-items: end;
    margin-right: 20px;
}

.box-premium {
    display: flex;
    width: 300px;
    height: 200px;
    border: 5px solid rgb(255, 255, 255);
    background: rgba(228, 231, 97, 0.826);
    border-radius: 10px;
    justify-content: center;
    align-items: end;
}

.box1 {
    display: flex;
    align-items: flex-start;
    justify-content: center;
    width: 120%;
}

.no-account{
    display: flex;
    justify-content: center;
    align-items: flex-start;
    padding: 20px;

}
</style>