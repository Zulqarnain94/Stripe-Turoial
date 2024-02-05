import React, { useState } from "react";
import { loadStripe } from "@stripe/stripe-js";

const PaymentForm: React.FC = () => {
  const [amount, setAmount] = useState<number>(0);

  const handleAmountChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const inputAmount = parseFloat(event.target.value);
    setAmount(isNaN(inputAmount) ? 0 : inputAmount);
  };

  const handlePayment = async () => {
    try {
      // Initialize Stripe.js with your public key
      const stripe = await loadStripe(
        "pk_test_51OdthoBtLyUDk5IywgHBe06AJYc1cuidNqi1FqAX6aUg9aZKfzkmYn3XodjGpeeP5eKvY1zexOJoSh8FFAisLG5i00cGFmfZJL"
      );

      // Make a request to your backend to create a checkout session
      const response = await fetch(
        "http://localhost:5127/Payment/create-checkout-session",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            amount: amount,
          }),
        }
        );

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      const responseData = await response.json();

      // Redirect the user to the Stripe Checkout page
      const { sessionId } = responseData;
      const { error } = await stripe.redirectToCheckout({
        sessionId: sessionId,
      });

      if (error) {
        console.error("Error redirecting to Checkout:", error);
        // Handle error as needed
      }
    } catch (error) {
      console.error("Error creating checkout session:", error);
      // Handle error as needed
    }
  };

  return (
    <div>
      <h1>Stripe Payment</h1>
      <div>
        <label htmlFor="amount">Amount:</label>
        <input
          type="text"
          id="amount"
          value={amount}
          onChange={handleAmountChange}
        />
      </div>
      <button onClick={handlePayment}>Pay</button>
    </div>
  );
};

export default PaymentForm;
