function test(param) {
    const t = 5;
    let y = param + t;

    console.log("Outer BlockScope - Y", y);

    for (let i = 0; i < y; i++) {
        const y = i; 
        console.log("BlockScope - Y", y);
    }

    console.log("Outer BlockScope - Y", y);

    const pole = [1, 2, 3, 4, 5, 6];
    for (const p of pole) console.log(p);
    for (let p of pole) console.log(p);
}

async function test_async(params)
{
    return new Promise((r) => {
        r(params + 5);
    });
}

async function test_await(param) {
    const t = await test_async(param);
}

var arrow_function1 = () => 5;
var arrow_function2 = () => { return 5; }
var arrow_function3 = (param) => param + 5;
var arrow_function4 = (param) => { return param + 5; }
var arrow_function5 = (param1, param2) => param1 + param2;
var arrow_function6 = (param1, param2) => { return param1 + param2; }
var arrow_function7 = async () => new Promise();