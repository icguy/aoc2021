function groupBy(list, keyGetter) {
    const map = new Map();
    list.forEach((item) => {
        const key = keyGetter(item);
        const collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        } else {
            collection.push(item);
        }
    });
    return map;
}


let fs = require("fs");

function readlines(path) {
    let file = fs.readFileSync("problems/" + path, { encoding: 'utf-8' });
    let lines = file.split(/\r\n?/);
    return lines;
}

function problem1part1() {
    let lines = readlines("p01.txt");
    let count = 0;
    for (let i = 1; i < lines.length; i++) {
        let curr = +lines[i];
        let prev = +lines[i - 1];
        if (curr - prev > 0)
            count++;
    }
    console.log(lines.length, count);
}

function p01p02() {
    let lines = readlines("p01.txt");
    let numbers = lines.map(a => +a);
    let count = 0;
    for (let i = 3; i < numbers.length; i++) {
        let curr = numbers[i];
        let prev = numbers[i - 3];
        if (curr - prev > 0)
            count++;
    }
    console.log(lines.length, count);
}

function p02p1() {
    let lines = readlines('p02.txt');
    let d = 0;
    let x = 0;
    for (let line of lines) {
        let [command, qty] = line.split(' ');
        switch (command) {
            case 'forward': x += +qty; break;
            case 'down': d += +qty; break;
            case 'up': d -= +qty; break;
        }
    }
    console.log(x, d, x * d);
}

function p02b() {
    let lines = readlines('p02.txt');
    let d = 0;
    let x = 0;
    let aim = 0
    for (let line of lines) {
        let [command, qty] = line.split(' ');
        switch (command) {
            case 'forward': x += +qty; d += aim * +qty; break;
            case 'down': aim += +qty; break;
            case 'up': aim -= +qty; break;
        }
    }
    console.log(x, d, x * d);
}

function p03a() {
    let lines = readlines('p03.txt');
    let acc = Array.from(lines[0]).map(a => 0);
    for (const line of lines) {
        for (let i = 0; i < line.length; i++) {
            const bit = line[i] === '1';
            if (bit)
                acc[i]++;
        }
    }
    let bits = acc.map(a => a > lines.length * 0.5 ? 0 : 1);
    let num = 0;
    let num2 = 0;
    let m = 1;
    for (let i = 0; i < bits.length; i++) {
        let curr = bits[bits.length - 1 - i] * m;
        num += curr;
        num2 += m - curr;
        m *= 2;
    }
    console.log(num, num2, num * num2);
}

function p03b() {
    let lines = readlines('p03.txt');
    let width = lines[0].length;

    let candidates = [...lines];
    for (let i = 0; i < width; i++) {
        if (candidates.length < 2)
            break;

        let num1 = candidates
            .filter(a => a.charAt(i) === '1')
            .length;
        let num0 = candidates.length - num1;
        let toKeep = num1 >= num0 ? '1' : '0';
        candidates = candidates.filter(a => a.charAt(i) === toKeep);
    }

    function bin2dec(bits) {
        let num = 0;
        let m = 1;
        for (let i = 0; i < bits.length; i++) {
            let curr = bits[bits.length - 1 - i] * m;
            num += curr;
            m *= 2;
        }
        return num;
    }
    let res1 = bin2dec(candidates[0]);

    candidates = [...lines];
    for (let i = 0; i < width; i++) {
        if (candidates.length < 2)
            break;

        let num1 = candidates
            .filter(a => a.charAt(i) === '1')
            .length;
        let num0 = candidates.length - num1;
        let toKeep = num1 >= num0 ? '0' : '1';
        candidates = candidates.filter(a => a.charAt(i) === toKeep);
    }
    let res2 = bin2dec(candidates[0]);

    console.log(res1, res2, res1 * res2);
}

function p10a() {
    let lines = readlines("p10.txt");
    let state = [];
    let errors = [];
    for (let line of lines) {
        state = [];
        lineloop: for (let char of line) {
            switch (char) {
                case '(': state.push(')'); break;
                case '{': state.push('}'); break;
                case '[': state.push(']'); break;
                case '<': state.push('>'); break;
                default:
                    if (state.pop() !== char) {
                        errors.push(char);
                        break lineloop;
                    }
            }
        }
    }

    let score = errors
        .map(a => {
            switch (a) {
                case ')': return 3;
                case ']': return 57;
                case '}': return 1197;
                case '>': return 25137;
            }
            return -1;
        })
        .reduce((sum, curr) => sum + curr, 0);
    console.log(errors, score);
}

function p10b() {
    let lines = readlines("p10.txt");
    let scores = [];
    for (let line of lines) {
        let state = [];
        state = [];
        let valid = true;
        lineloop: for (let char of line) {
            switch (char) {
                case '(': state.push(')'); break;
                case '{': state.push('}'); break;
                case '[': state.push(']'); break;
                case '<': state.push('>'); break;
                default:
                    if (state.pop() !== char) {
                        valid = false;
                        break lineloop;
                    }
            }
        }
        if (valid) {
            let score = 0;
            while (state.length > 0) {
                let char = state.pop();
                score *= 5;
                switch (char) {
                    case ')': score += 1; break;
                    case ']': score += 2; break;
                    case '}': score += 3; break;
                    case '>': score += 4; break;
                }
            }
            scores.push(score);
        }
    }
    let result = scores.sort((a, b) => a - b)[(scores.length - 1) / 2];
    console.log(result);
}

function p14a() {
    let lines = readlines("p14.txt");
    let init = lines[0];
    lines.splice(0, 2);
    let rules = [];
    for (let line of lines) {
        // AA -> B
        rules.push({
            first: line[0],
            second: line[1],
            toAdd: line[6]
        });
    }

    function transform(poly) {
        let newPoly = [poly[0]];
        for (let i = 1; i < poly.length; i++) {
            const el1 = poly[i - 1];
            const el2 = poly[i];
            let rule = rules.find(r => r.first === el1 && r.second === el2);
            if (rule) {
                newPoly.push(rule.toAdd);
            }
            newPoly.push(el2);
        }
        return newPoly;
    }

    let resultPoly = init;
    for (let i = 0; i < 10; i++) {
        resultPoly = transform(resultPoly);
    }

    let grouped = groupBy(resultPoly, a => a);
    let all = [...grouped.entries()]
        .map(([char, instances]) => ([char, instances.length]))
        .sort(([_, a], [__, b]) => a - b);
    let result = all[all.length - 1][1] - all[0][1];
    console.log(result);
}

function p14b() {
    let lines = readlines("p14.txt");
    let maxGen = 20;
    let state = [...lines[0]].map(a => ({ gen: 0, char: a }));
    lines.splice(0, 2);
    let rules = [];
    for (let line of lines) {
        // AA -> B
        rules.push({
            first: line[0],
            second: line[1],
            toAdd: line[6]
        });
    }

    let counts = new Map();
    function increment(char) {
        if (counts.has(char)) counts.set(char, counts.get(char) + 1);
        else counts.set(char, 1);
    }


    while (true) {
        if (state.length === 1) {
            increment(state[0].char);
            break;
        }
        if (state.length === 0) {
            break;
        }

        let p1 = state[0];
        let p2 = state[1];

        if (p2.gen === maxGen) {
            increment(p1.char);
            increment(p2.char);
            state.splice(0, 2);
        }
        else {
            let rule = rules.find(r => r.first === p1.char && r.second === p2.char);
            state.splice(1, 0, { char: rule.toAdd, gen: (p1.gen > p2.gen ? p1.gen : p2.gen) + 1 });
        }
    }

    console.log(counts.entries());
}

function p14b_v2() {
    let lines = readlines("p14.txt");
    let maxGen = 40;
    let init = lines[0];
    lines.splice(0, 2);
    let rules = [];
    for (let line of lines) {
        // AB -> C
        let a = line.charAt(0);
        let b = line.charAt(1);
        let c = line.charAt(6);
        rules.push({
            prev: a + b,
            next: [a + c, c + b]
        });
    }

    function increase(m, char, amount) {
        if (m.has(char)) m.set(char, m.get(char) + amount);
        else m.set(char, amount);
    }

    let initPairs = [];
    for (let i = 1; i < init.length; i++) {
        initPairs.push(init.substr(i - 1, 2));
    }

    let curr = groupBy(initPairs, a => a);
    for (const [k, v] of curr.entries()) {
        curr.set(k, v.length);
    }

    function iterate() {
        let next = new Map();
        for (const [k, v] of curr.entries()) {
            let rule = rules.find(r => r.prev === k);
            if (rule) {
                increase(next, rule.next[0], v);
                increase(next, rule.next[1], v);
            }
            else {
                increase(next, k, v);
            }
        }
        return next;
    }

    for (let i = 0; i < maxGen; i++) {
        curr = iterate(curr);
    }

    let letterCount = new Map();
    for (let [k, v] of curr.entries()) {
        increase(letterCount, k[0], v);
        increase(letterCount, k[1], v);
    }
    increase(letterCount, init[0], 1);
    increase(letterCount, init[init.length - 1], 1);

    //console.log(letterCount.entries());
    let realCounts = [...letterCount.values()]
        .map(a => a / 2)
        .sort((a, b) => a - b);
    let min = realCounts[0];
    let max = realCounts[realCounts.length - 1];
    console.log(min, max, max - min);
}


p14b_v2();
