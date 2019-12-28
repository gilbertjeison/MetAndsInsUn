import axios from 'axios';

const baseDomain = 'http://localhost:5000';
const baseDomain_loc = 'http://localhost/metins';
const baseDomainProd = 'http://metinsman.digitoolsunilever.com';

const baseUrlDev = `${baseDomain}/api`;
const baseUrl = `${baseDomainProd}/api`;
const baseUrlloc = `${baseDomain_loc}/api`;

// export default axios.create({
//     baseURL:baseUrlDev
// });

export default axios.create({
    baseURL:baseUrlloc
});

// export default axios.create({
//     baseURL:baseUrl
// });