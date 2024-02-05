import React, { useState } from "react";
import { loadStripe } from "@stripe/stripe-js";

const stripePromise = loadStripe(
  "pk_test_51OdthoBtLyUDk5IywgHBe06AJYc1cuidNqi1FqAX6aUg9aZKfzkmYn3XodjGpeeP5eKvY1zexOJoSh8FFAisLG5i00cGFmfZJL"
);

const PaymentForm = () => {
  const [amount, setAmount] = useState("");
  const [customerName, setCustomerName] = useState("");
  const [customerEmail, setCustomerEmail] = useState("");
  const [productName, setProductName] = useState("");

  const handleSubmit = async (event) => {
    event.preventDefault();
    const stripe = await stripePromise;
    const response = await fetch(
      "http://localhost:5185/payment/create-checkout-session",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          amount,
          customerName,
          customerEmail,
          productName,
        }),
      }
    );
    const session = await response.json();

    // Correctly access the sessionId from the session object
    const sessionId = session.sessionId;

    const result = await stripe.redirectToCheckout({
      sessionId, // Use this corrected variable
    });

    if (result.error) {
      console.log(result.error.message);
            
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Product Name"
        value={productName}
        onChange={(e) => setProductName(e.target.value)}
      />
      <input
        type="text"
        placeholder="Name"
        value={customerName}
        onChange={(e) => setCustomerName(e.target.value)}
      />
      <input
        type="email"
        placeholder="Email"
        value={customerEmail}
        onChange={(e) => setCustomerEmail(e.target.value)}
      />
      <input
        type="number"
        placeholder="Amount"
        value={amount}
        onChange={(e) => setAmount(e.target.value)}
      />
      <button type="submit">Pay</button>
    </form>
  );
};

export default PaymentForm;

// const stripe = await loadStripe(
//   "pk_test_51OdthoBtLyUDk5IywgHBe06AJYc1cuidNqi1FqAX6aUg9aZKfzkmYn3XodjGpeeP5eKvY1zexOJoSh8FFAisLG5i00cGFmfZJL"
// );

// const response = await fetch(
//   "http://localhost:5185/payment/create-checkout-session",
