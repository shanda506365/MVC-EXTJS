// RSA, a suite of routines for performing RSA public-key computations in
// JavaScript.
//
// Requires BigInt.js and Barrett.js.
//
// Copyright 1998-2005 David Shapiro.
//http://www.ohdave.com/
// You may use, re-use, abuse, copy, and modify this code to your liking, but
// please keep this header.
//
// Thanks!
//
// Dave Shapiro
// dave@ohdave.com

/*Ê¹ÓÃ·¶Àý
var RASkey = createRSAKey(512);
 var str = "111dds";
    var str1 = encryptedString(RASkey,str);
    var str2 = decryptedString(RASkey,str1);
    alert(str1+"  "+str2);
*/

function RSAKeyPair(encryptionExponent, decryptionExponent, modulus) {
    this.e = biFromHex(encryptionExponent);
    this.d = biFromHex(decryptionExponent);
    this.m = biFromHex(modulus);
    // We can do two bytes per digit, so
    // chunkSize = 2 * (number of digits in modulus - 1).
    // Since biHighIndex returns the high index, not the number of digits, 1 has
    // already been subtracted.
    this.chunkSize = 2 * biHighIndex(this.m);
    this.radix = 16;
    this.barrett = new BarrettMu(this.m);
}

function twoDigit(n) {
    return (n < 10 ? "0" : "") + String(n);
}

function encryptedString(key, s)
    // Altered by Rob Saunders (rob@robsaunders.net). New routine pads the
    // string after it has been converted to an array. This fixes an
    // incompatibility with Flash MX's ActionScript.
{
    var a = new Array();
    var sl = s.length;
    var i = 0;
    while (i < sl) {
        a[i] = s.charCodeAt(i);
        i++;
    }

    while (a.length % key.chunkSize != 0) {
        a[i++] = 0;
    }

    var al = a.length;
    var result = "";
    var j, k, block;
    for (i = 0; i < al; i += key.chunkSize) {
        block = new BigInt();
        j = 0;
        for (k = i; k < i + key.chunkSize; ++j) {
            block.digits[j] = a[k++];
            block.digits[j] += a[k++] << 8;
        }
        var crypt = key.barrett.powMod(block, key.e);
        var text = key.radix == 16 ? biToHex(crypt) : biToString(crypt, key.radix);
        result += text + " ";
    }
    return result.substring(0, result.length - 1); // Remove last space.
}

function decryptedString(key, s) {
    var blocks = s.split(" ");
    var result = "";
    var i, j, block;
    for (i = 0; i < blocks.length; ++i) {
        var bi;
        if (key.radix == 16) {
            bi = biFromHex(blocks[i]);
        } else {
            bi = biFromString(blocks[i], key.radix);
        }
        block = key.barrett.powMod(bi, key.d);
        for (j = 0; j <= biHighIndex(block) ; ++j) {
            result += String.fromCharCode(block.digits[j] & 255,
			block.digits[j] >> 8);
        }
    }
    // Remove trailing null, if any.
    if (result.charCodeAt(result.length - 1) == 0) {
        result = result.substring(0, result.length - 1);
    }
    return result;
}

function createRSAKey(keySize) {
    var rsaKey;
    if (keySize == 128) {
        setMaxDigits(19);
        rsaKey = new RSAKeyPair("11", "3cfe098b538f4f2a48d949ba0363e41", "cf5fba0ce8e7406c5098360df293acb");
    } else if (keySize == 256) {
        setMaxDigits(35);
        rsaKey = new RSAKeyPair("11", "2af4f7bf9057d628a55a9a05653a5cfe42df74cd57d1cb2476053d742e00f31", "920db0be845dd823cbcda5458b6009019c773b66920b36aec9c5dafcc17d29d");
    } else if (keySize == 512) {
        setMaxDigits(67);
        rsaKey = new RSAKeyPair("11", "ab1be7bfa5602ade8c6c96a3cb1605f7df5d765bb8807c2e1fae52df88a6641d7121364bce03196a115d2e59cb65e599f9b5d4cd349907b1ab52722c1c57ec1", "dfc207abd842ae36b7a1b14c5857e06b868de98ba280a263b3466c5f63ed47df88ddd9340543a9a53face95d180741544e53de981e17748c59ed946ad18b189");
    } else if (keySize == 1024) {
        setMaxDigits(131);
        rsaKey = new RSAKeyPair("11", "22d7bd8f74587ec2bac628e8160265e58653432a03aeb117e75eb29bdd9c14e390dd1612626f6479ea1300d5505982d282be1ed00aebfb8afd943343131921b1a8f1f63ca420d6137f5100ea5b471ca1b6e14dce6cde35edd4944ed1e83e1b8c1c42835d5ab52522ead1c8df42af1384852c67bdf89b24ad3d1804b408471", "250539686b9e06aee6728b7697628c43deb8775ca3e99c2965d49dc59b75d631c9eae77388965ac188b430e2a55f1affaaea00bd0b9abb43ad6d7677444ab453dae40c9072b339f32f9f5028edad6d2970fd54f0efa8e56ecb12c616a33d1c91bd84df590bd05973037c1e48b7ca52ef1d8138b92fda6a14e6dc7239d16a89");
    } else if (keySize == 2048) {
        setMaxDigits(259);
        rsaKey = new RSAKeyPair("11", "6c524425e9f69bb94622cb2d16a8c6af19af1b1ee67405d5d7cc3ef23d52b55ac0d62df9438580f76b304fcdc7feb0589558bc905134b807b0372e701819253e810bc7d0894c0533d3771eea8697f63c848f023457fe1c14f1318be05b0ec40278d332489ca89b386928b5c561f7132cfc3478c06ce5125801bbb5d8fdd9c91ad5f83490603df1e73d563c46ca051f2879943b632b1f275b9e3c26d008eb468103e80eceb11fcf9878c18ef89b3eccde0c45965d2e2b719d663ee7710dac71ea02cf38b75f926f84d750ada3b7e95b4e383caf390bcbc2933459859940f2d0ec3db631cda37b6f83380c4dfd846df949457903841c59d56be8a89fb582bb941", "10710eea53832559d612fed6d802c2ba93e601d4b0b19c50755392b27b97fb86eafbf01efa3fb14a20450c1cf2ed83e8dfcfc131557800812abf3b9eba83d114ea71c9c1f044ae81026214b14fdba316e667fe0c843691fa092e60a8e93ff49bcdc4a0c67335054646d19b971a4c5c0daf6c892f7e3e32c8c967eb9a143ec5624bf7101cdb902b4b176cd45ce22d6acb5be5f140498888a95dca24bc203629e70f27a85c2cbe1c3a0b7d6f683971b3e6bb4fa7ff8dd1342f55212ee96c5d868aef0ed84cfd8c6b57bae0c80483c10b9469db14f5ec8756e5685dae2d100aad7f2326c1b794510ba7ed7194562ad7c319f65e590550312da055e4ae3e954211ab");
    }
    return rsaKey;
}

var RASkey = createRSAKey(128);